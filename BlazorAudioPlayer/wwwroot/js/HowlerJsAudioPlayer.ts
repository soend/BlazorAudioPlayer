/*!
 * Simple audio player based on the original Howler.js Audio Player Demo
 * https://github.com/goldfire/howler.js/blob/master/examples/player/player.js
 */

/// <reference types="howler"/>

let audioPlayer: AudioPlayer;
let hapDotNet;

class PlaylistItem {
    id: number;
    title: string;
    file: string;
    howl: Howl;

    constructor(title, file) {
        this.title = title;
        this.file = file;
    }
}

class AudioPlayer {
    playlist: { [id: number]: PlaylistItem } = {};
    index = 0;

    constructor(playlist) {
        playlist.forEach(item => {
            this.playlist[item.id] = new PlaylistItem(item.title, item.file);
        });
    }

    play(index: number = 0): string {
        var sound;
        console.log('playing id:' + index);

        if (index >= Object.keys(this.playlist).length) {
            console.log('track does not exist');
            return;
        }
        
        var data = this.playlist[index];

        // If we already loaded this track, use the current one.
        // Otherwise, setup and load a new Howl
        if (data.howl) {
            sound = data.howl;
        } else {
            sound = data.howl = new Howl({
                src: data.file,
                html5: true, // Force to HTML5 so that the audio can stream in (best for large files).
                onplay: () => {
                    // Start upating the progress of the track.
                    // We user this instead of timer in blazor code because it more optimized
                    requestAnimationFrame(this.step.bind(this));

                    hapDotNet.invokeMethodAsync('RaiseOnPlay', sound.duration());
                },
                onload: () => {
                    hapDotNet.invokeMethodAsync('RaiseOnLoad');
                },
                onend: () => {
                    hapDotNet.invokeMethodAsync('RaiseOnEnd');
                },
                onpause: () => {
                    hapDotNet.invokeMethodAsync('RaiseOnPause');
                },
                onstop: () => {
                    hapDotNet.invokeMethodAsync('RaiseOnStop');
                },
                onseek: () => {
                    // Start upating the progress of the track.
                    requestAnimationFrame(this.step.bind(this));
                    hapDotNet.invokeMethodAsync('RaiseOnSeek');
                }
            });
        }

        // Begin playing the sound.
        sound.play();

        // Keep track of the index we are currently playing.
        this.index = index;

        return sound.state();
    }

    pause() {
        // Get the Howl we want to manipulate.
        var sound = this.playlist[this.index].howl;

        // Puase the sound.
        sound.pause();
    }

    stop(index) {
        if (this.playlist[index].howl) {
            this.playlist[index].howl.stop();
        }
    }

    volume(val) {
        // Update the global volume (affecting all Howls).
        Howler.volume(val);
    }

    seek(per) {
        // Get the Howl we want to manipulate.
        var sound = this.playlist[this.index].howl;

        // Convert the percent into a seek position.
        if (sound.playing()) {
            sound.seek(sound.duration() * per);
        }
    }

    step() {
        // Get the Howl we want to manipulate.
        var sound = this.playlist[this.index].howl;

        // Determine our current seek position.
        var seek = sound.seek() || 0;

        if (typeof seek == 'number')
            hapDotNet.invokeMethodAsync('RaiseOnStep', seek as number);

        // If the sound is still playing, continue stepping.
        if (sound.playing()) {
            requestAnimationFrame(this.step.bind(this));
        }
    }
}

// Interop calls
function createPlayer(playlist, dotNetHelper): void {
    hapDotNet = dotNetHelper;
    audioPlayer = new AudioPlayer(playlist);
}

function playHowl(index): string {
    return audioPlayer.play(index);
}

function pauseHowl(): void {
    audioPlayer.pause();
}

function stopHowl(index): void {
    audioPlayer.stop(index);
}

function seekHowl(position): void {
    audioPlayer.seek(position);
}

function volumeHowl(vol): void {
    audioPlayer.volume(vol);
}