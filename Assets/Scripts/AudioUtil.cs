using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
public static class AudioUtil
{

    // Method to trim silence from the audio clip
    public static AudioClip TrimSilence(AudioClip clip, float min)
    {
        var samples = new float[clip.samples];
        clip.GetData(samples, 0);
        return TrimSilence(new List<float>(samples), min, clip.channels, clip.frequency);
    }

    // Method to trim silence from a list of audio samples
    public static AudioClip TrimSilence(List<float> samples, float min, int channels, int hz, bool stream = false)
    {
        int i;

        // Trim leading silence
        for (i = 0; i < samples.Count; i++)
        {
            if (Mathf.Abs(samples[i]) > min)
            {
                break;
            }

        }
        samples.RemoveRange(0, i);

        // Trim trailing silence
        for (i = samples.Count - 1; i > 0; i--)
        {
            if (Mathf.Abs(samples[i]) > min)
            {
                break;
            }
        }

        if (samples.Count == 0) { 
            return null;
        }

        if (i + 2 < samples.Count)
        {
            samples.RemoveRange(i + 2, samples.Count - (i + 2));
        }
        else {
            samples.RemoveRange(i , samples.Count - i);
        }



        // Create a new AudioClip with the trimmed samples
        AudioClip clip = AudioClip.Create("TempClip", samples.Count, channels, hz, stream);
        clip.SetData(samples.ToArray(), 0);

        return clip;
    }

    // Method to convert AudioClip to Base64 Linear16 PCM
    public static string ConvertToBase64Linear16(AudioClip clip)
    {
        using (var memoryStream = new MemoryStream())
        using (var writer = new BinaryWriter(memoryStream))
        {
            int sampleRate = clip.frequency;
            int channels = clip.channels;
            int samples = clip.samples;

            // Convert float samples to 16-bit PCM
            float[] floatSamples = new float[samples * channels];
            clip.GetData(floatSamples, 0);

            foreach (var sample in floatSamples)
            {
                short pcmSample = (short)(sample * short.MaxValue); // Convert to Linear16 PCM
                writer.Write(pcmSample);
            }

            byte[] pcmData = memoryStream.ToArray();

            memoryStream.Close();
            return Convert.ToBase64String(pcmData);
        }
    }
}
