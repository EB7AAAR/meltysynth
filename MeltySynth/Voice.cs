﻿using System;

namespace MeltySynth
{
    internal sealed class Voice
    {
        private Synthesizer synthesizer;

        private VolumeEnvelope volumeEnvelope;
        private ModulationEnvelope modulationEnvelope;
        private Generator generator;

        private float[] block;

        private RegionPair region;
        private int channel;
        private int key;
        private int velocity;

        internal Voice(Synthesizer synthesizer)
        {
            this.synthesizer = synthesizer;

            volumeEnvelope = new VolumeEnvelope(synthesizer);
            modulationEnvelope = new ModulationEnvelope(synthesizer);
            generator = new Generator(synthesizer);

            block = new float[synthesizer.BlockSize];
        }

        internal void Start(RegionPair region, int channel, int key, int velocity)
        {
            volumeEnvelope.Start(region, key, velocity);
            modulationEnvelope.Start(region, key, velocity);
            generator.Start(synthesizer.SoundFont.WaveData, region);

            this.region = region;
            this.channel = channel;
            this.key = key;
            this.velocity = velocity;
        }

        internal void End()
        {
            volumeEnvelope.Release();

            if (region.SampleModes == LoopMode.LoopUntilNoteOff)
            {
                generator.Release();
            }
        }

        internal bool Process()
        {
            if (!volumeEnvelope.Process(synthesizer.BlockSize))
            {
                return false;
            }

            if (!generator.Process(block, key))
            {
                return false;
            }

            modulationEnvelope.Process(synthesizer.BlockSize);

            var test = volumeEnvelope.Value;
            for (var t = 0; t < block.Length; t++)
            {
                block[t] *= test;
            }

            return true;
        }

        internal float[] Block => block;
        internal int Channel => channel;
        internal int Key => key;
        internal int Velocity => velocity;
    }
}
