﻿using System;
using NAudio.Wave;
using MeltySynth;

class Program
{
    static void Main()
    {
        using (var waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
        {
            var player = new MidiSampleProvider("TimGM6mb.sf2");
            waveOut.Init(player);
            waveOut.Play();

            // Load the MIDI file.
            var midiFile = new MidiFile(@"C:\Windows\Media\flourish.mid");

            // Play the MIDI file.
            player.Play(midiFile, true);

            // Wait until any key is pressed.
            Console.ReadKey();
        }
    }
}
