using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SETHD.Echo
{
    public class AudioSourceProvider
    {
        public int Count => disables.Count;

        private readonly Transform group;
        private readonly Stack<AudioSource> disables;
        private readonly PlaceholderFactory<Transform, AudioSource> factory;

        public AudioSourceProvider(Transform group, PlaceholderFactory<Transform, AudioSource> factory)
        {
            this.group = group;
            this.factory = factory;
            disables = new Stack<AudioSource>();
        }

        public AudioSource Rent()
        {
            var audioSource = disables.Count <= 0 ? factory.Create(group) : disables.Pop();
            return audioSource;
        }

        public void Return(AudioSource source)
        {
            disables.Push(source);
        }
    }
}