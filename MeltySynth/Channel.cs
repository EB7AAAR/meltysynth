﻿using System;

namespace MeltySynth
{
    internal sealed class Channel
    {
        private Synthesizer synthesizer;
        private bool isPercussionChannel;

        private float[] blockLeft;
        private float[] blockRight;

        private int bankNumber;
        private int patchNumber;

        private short modulation;
        private short volume;
        private short pan;
        private short expression;

        private float pitchBendRange;
        private float pitchBend;

        internal Channel(Synthesizer synthesizer, bool isPercussionChannel)
        {
            this.synthesizer = synthesizer;
            this.isPercussionChannel = isPercussionChannel;

            blockLeft = new float[synthesizer.BlockSize];
            blockRight = new float[synthesizer.BlockSize];

            Reset();
        }

        public void Reset()
        {
            bankNumber = isPercussionChannel ? 128 : 0;
            patchNumber = 0;

            modulation = 0;
            volume = 100 << 7;
            pan = 64 << 7;
            expression = 127 << 7;

            pitchBendRange = 2F;
            pitchBend = 0F;
        }

        public void SetBank(int value)
        {
            bankNumber = value;

            if (isPercussionChannel)
            {
                bankNumber += 128;
            }
        }

        public void SetPatch(int value)
        {
            patchNumber = value;
        }

        public void SetModulationCourse(int value)
        {
            modulation = (short)((modulation & 0x7F) | (value << 7));
        }

        public void SetModulationFine(int value)
        {
            modulation = (short)((modulation & 0xFF80) | value);
        }

        public void SetVolumeCourse(int value)
        {
            volume = (short)((volume & 0x7F) | (value << 7));
        }

        public void SetVolumeFine(int value)
        {
            volume = (short)((volume & 0xFF80) | value);
        }

        public void SetPanCourse(int value)
        {
            pan = (short)((pan & 0x7F) | (value << 7));
        }

        public void SetPanFine(int value)
        {
            pan = (short)((pan & 0xFF80) | value);
        }

        public void SetExpressionCourse(int value)
        {
            expression = (short)((expression & 0x7F) | (value << 7));
        }

        public void SetExpressionFine(int value)
        {
            expression = (short)((expression & 0xFF80) | value);
        }

        public void SetPitchBend(int value1, int value2)
        {
            pitchBend = ((value1 | (value2 << 7)) - 8192) / 8192F;
        }

        public Preset Preset => synthesizer.GetPreset(bankNumber, patchNumber);

        public float Modulation => modulation * (50F / 16383F);
        public float Volume => volume / 16383F;
        public float Pan => pan * (100F / 16383F) - 50F;
        public float Expression => expression / 16383F;

        public float PitchBend => pitchBendRange * pitchBend;
    }
}
