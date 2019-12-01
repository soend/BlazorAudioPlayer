using System;
using BlazorAudioPlayer.Models;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorAudioPlayer
{
    public class HowlerJsAudioPlayerInterop
    {
        public Func<double, Task> OnPlay { get; set; }
        public Func<Task> OnLoad { get; set; }
        public Func<Task> OnEnd { get; set; }
        public Func<Task> OnPause { get; set; }
        public Func<Task> OnStop { get; set; }
        public Func<Task> OnSeek { get; set; }
        public Func<double, Task> OnStep { get; set; }

        public async Task CreateAudioPlayer(IJSRuntime jsRuntime, List<PlaylistItem> playlist)
        {
            await jsRuntime.InvokeAsync<Task>("createPlayer", playlist, DotNetObjectReference.Create(this));
        }

        public async ValueTask<HowlStateEnum> Play(IJSRuntime jsRuntime, int index)
        {
            return await jsRuntime.InvokeAsync<HowlStateEnum>("playHowl", index);
        }

        public async Task Pause(IJSRuntime jsRuntime)
        {
            await jsRuntime.InvokeAsync<Task>("pauseHowl");
        }

        public async Task Stop(IJSRuntime jsRuntime, int index)
        {
            await jsRuntime.InvokeAsync<Task>("stopHowl", index);
        }

        public async Task Previous(IJSRuntime jsRuntime)
        {
            await jsRuntime.InvokeAsync<Task>("previousHowl");
        }

        public async Task Next(IJSRuntime jsRuntime)
        {
            await jsRuntime.InvokeAsync<Task>("nextHowl");
        }

        public async Task SkipTo(IJSRuntime jsRuntime, int index)
        {
            await jsRuntime.InvokeAsync<Task>("skipToHowl", index);
        }

        public async Task Volume(IJSRuntime jsRuntime, double volume)
        {
            await jsRuntime.InvokeAsync<Task>("volumeHowl", volume);
        }

        public async Task Seek(IJSRuntime jsRuntime, double position)
        {
            await jsRuntime.InvokeAsync<Task>("seekHowl", position);
        }

        [JSInvokable]
        public async void RaiseOnPlay(double duration)
        {
            if (OnPlay != null)
                await OnPlay.Invoke(duration);
        }

        [JSInvokable]
        public async void RaiseOnLoad()
        {
            if (OnLoad != null)
                await OnLoad.Invoke();
        }

        [JSInvokable]
        public async void RaiseOnEnd()
        {
            if (OnEnd != null)
                await OnEnd.Invoke();
        }

        [JSInvokable]
        public async void RaiseOnPause()
        {
            if (OnPause != null)
                await OnPause.Invoke();
        }

        [JSInvokable]
        public async void RaiseOnStop()
        {
            if (OnStop != null)
                await OnStop.Invoke();
        }

        [JSInvokable]
        public async void RaiseOnSeek()
        {
            if (OnSeek != null)
                await OnSeek.Invoke();
        }

        [JSInvokable]
        public async void RaiseOnStep(double seek)
        {
            if (OnStep != null)
                await OnStep.Invoke(seek);
        }
    }
}
