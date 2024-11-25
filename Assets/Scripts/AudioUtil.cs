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

    // Convet base64 to AudioClip
    public static AudioClip ConvertToAudioClip(byte[] audioData)
    {
        try
        {
            // Parse the WAV file header
            using (MemoryStream memoryStream = new MemoryStream(audioData))
            using (BinaryReader reader = new BinaryReader(memoryStream))
            {
                // WAV header checks
                string riff = new string(reader.ReadChars(4));
                if (riff != "RIFF") throw new Exception("Invalid WAV file: missing RIFF header.");

                reader.ReadInt32(); // Skip file size
                string wave = new string(reader.ReadChars(4));
                if (wave != "WAVE") throw new Exception("Invalid WAV file: missing WAVE header.");

                // Read format chunk
                string fmt = new string(reader.ReadChars(4));
                if (fmt != "fmt ") throw new Exception("Invalid WAV file: missing fmt header.");

                int fmtSize = reader.ReadInt32();
                int audioFormat = reader.ReadInt16();
                if (audioFormat != 1) throw new Exception("Unsupported WAV file: must be PCM format.");

                int numChannels = reader.ReadInt16();
                int sampleRate = reader.ReadInt32();
                reader.ReadInt32(); // Byte rate (not needed)
                reader.ReadInt16(); // Block align (not needed)
                int bitsPerSample = reader.ReadInt16();

                // Read data chunk
                string dataChunkId = new string(reader.ReadChars(4));
                while (dataChunkId != "data")
                {
                    reader.BaseStream.Position += reader.ReadInt32();
                    dataChunkId = new string(reader.ReadChars(4));
                }

                int dataSize = reader.ReadInt32();
                byte[] audioBuffer = reader.ReadBytes(dataSize);

                // Create AudioClip
                int sampleCount = dataSize / (bitsPerSample / 8);
                float[] audioDataFloat = new float[sampleCount];
                for (int i = 0; i < sampleCount; i++)
                {
                    if (bitsPerSample == 16)
                        audioDataFloat[i] = BitConverter.ToInt16(audioBuffer, i * 2) / 32768f;
                    else if (bitsPerSample == 8)
                        audioDataFloat[i] = (audioBuffer[i] - 128) / 128f;
                }

                AudioClip audioClip = AudioClip.Create("GoogleTTSAudio", sampleCount, numChannels, sampleRate, false);
                audioClip.SetData(audioDataFloat, 0);

                return audioClip;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error converting audio data: {ex.Message}");
            return null;
        }
    }
}
