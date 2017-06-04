using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MIDIReader : MonoBehaviour
{
    /// <summary>
    /// 128-143, Byte (key #), Byte (Off Vel.)              Note OFF Channel 1-16
    /// 144-159, Byte (key #), Byte (On Vel.)               Note ON  Channel 1-16
    /// 160-175, Byte (key #), Byte (Pressure(0-127))       Note Poly Key Pressure
    /// 176-191, Control # (0-127), Control Val (0-127),    Note Control Change
    /// 192-207, Program # (0-127), Program # (0-127), N/A, Note Program Change
    /// 208-223, Pressure Val. (0-127), N/A,                Note Mono Key Pressure (Channel Pressure)
    /// 224-239, Range (LSB), Range (MSB),                  Note Pitch Bend
    /// 240-255, Manufacturer's ID, Model ID,               Note System
    /// </summary>
    /// 
    public string midi = "Assets/";

    uint Counter = 0;
    byte commandByte = 0;
    byte noteByte = 0;
    byte velocityByte = 0;

    byte noteOn = 144;
    byte[] MIDI;

    int Index = 0;
    int Length = 0;

    public bool PrintInfo = false;
    public bool Channels = true;
    public bool NotesInHex = false;
    public bool OnlyChannelOne = false;

    // Use this for initialization
    void Start()
    {
        MIDI = File.ReadAllBytes(midi);
        Debug.Log("MIDI Length : " + MIDI.Length.ToString());

        Debug.Log(((char)MIDI[0]).ToString() + ((char)MIDI[1]).ToString() + ((char)MIDI[2]).ToString() + ((char)MIDI[3]).ToString());
        Length = (MIDI[4] << 24) | (MIDI[5] << 16) | (MIDI[6] << 8) | MIDI[7];
        int format = (MIDI[8] << 8) | MIDI[9];
        int tracks = (MIDI[10] << 8) | MIDI[11];
        int division = (MIDI[12] << 8) | MIDI[13];

        Debug.Log("Length : " + Length.ToString());
        Debug.Log("Format : " + format.ToString());
        Debug.Log("Tracks : " + tracks.ToString());

        if ((division & (1 << 15)) == 0)
            Debug.Log("Delta Time : " + division.ToString());
        else
            Debug.Log("Ticks/frame");

        Index = 14;

        if (PrintInfo)
        {
            for (int i = 0; i < MIDI.Length; i++)
            {
                //Debug.Log((i + 1) + " : " + MIDI[i]);
                if (Channels)
                {
                    if (MIDI[i] >= 144 && MIDI[i] <= 159)
                    {
                        if (OnlyChannelOne)
                        {
                            if (MIDI[i] != 144)
                                continue;
                        }

                        if (NotesInHex)
                            Debug.Log("Channel " + (MIDI[i] - 143).ToString() + " On, Note " + MIDI[i + 1].ToString("X") + ", Velocity " + MIDI[i + 2]);
                        else
                            Debug.Log("Channel " + (MIDI[i] - 143).ToString() + " On, Note " + MIDI[i + 1].ToString() + ", Velocity " + MIDI[i + 2]);
                        continue;
                    }

                    if (MIDI[i] >= 128 && MIDI[i] <= 143)
                    {
                        if (OnlyChannelOne)
                        {
                            if (MIDI[i] != 128)
                                continue;
                        }
                        if (NotesInHex)
                            Debug.Log("Channel " + (MIDI[i] - 127).ToString() + " Off, Note " + MIDI[i + 1].ToString("X") + ", Velocity " + MIDI[i + 2]);
                        else
                            Debug.Log("Channel " + (MIDI[i] - 127).ToString() + " Off, Note " + MIDI[i + 1].ToString() + ", Velocity " + MIDI[i + 2]);
                        continue;
                    }
                }
            }
        }
    }
}
