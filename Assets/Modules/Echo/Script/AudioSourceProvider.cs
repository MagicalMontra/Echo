using Zenject;
using UnityEngine;
using System.Collections.Generic;

namespace SETHD.Echo
{
    public class AudioSourceProvider
    {
        private readonly AudioSourceFactory factory;
        private readonly Stack<AudioSource> disables;

        public AudioSourceProvider(AudioSourceFactory factory)
        {
            this.factory = factory;
            disables = new Stack<AudioSource>();
        }

        public AudioSource Rent()
        {
            var audioSource = disables.Count <= 0 ? factory.Create() : disables.Pop();
            return audioSource;
        }

        public void Return(AudioSource source)
        {
            disables.Push(source);
        }
    }
}