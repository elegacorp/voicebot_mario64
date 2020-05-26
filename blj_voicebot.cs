// Authored by Elega Corporation, 2020.
// This code is under the MIT license: please see the LICENSE file regarding usage of this code.

// Documentation for Voicebot scripted functions in macros can be found:
// https://www.voicebot.net/ScriptedFunctions/Help

using System;
using System.Drawing;

public static class VoiceBotScript
{
    public static void wait(int timeToWait)
    {
        // To wait, just loop and count while 
        // waiting to reach the timeToWait
        // This hopefully translates to a delay
        // in some number of milliseconds when
        // executing...
        for (int i = 0; i < timeToWait; i++)
            continue;
    }
    
    public static void Run(IntPtr windowHandle)
    {
        // This allows a flag to be set for wiring up to a stop command
        BFS.ScriptSettings.WriteValueBool("Repeat_Jump", true);

        // The number of times to mash the jump button
        int numberOfKeyPresses = 40000;
        int currentKeyPress = 0; // Initialize the starting key press at zero (or the first value)
        int periodToWait = 25; // Number of cycles between key presses

        // Then, break up the key press counts into 4 quartiles, so
        // that when progressing through the key presses, it is possible
        // to accelerate the button mashing to a higher speed.
        int keyPressQuartileOne = numberOfKeyPresses / 4;
        int keyPressQuartileTwo = numberOfKeyPresses / 2;
        int keyPressQuartileThree = keyPressQuartileOne * 3;

        // These true/false boolean flags allow us to know
        // when we have hit the critical quartiles for 
        // acceleration and then, once true, avoid repeatedly
        // manipulating the periodToWait variable.
        bool hitQuartileTwo = false;
        bool hitQuartileThree = false;
        
        // While we have not run a stop command verbally, or while
        // we have not reached the maximum number of button presses...
        while(BFS.ScriptSettings.ReadValueBool("Repeat_Jump")
            && currentKeyPress < numberOfKeyPresses)
        {
            // Mash the 'M' key (or the jump button)
            BFS.Input.SendKeyDown("M");
            BFS.Input.SendKeyUp("M");

            // Count this as a key press against the maximum
            currentKeyPress += 1;

            // If we've hit the second quartile, increase our acceleration
            // by reducing the delay in cycles between key presses
            if (currentKeyPress <= keyPressQuartileTwo && !hitQuartileTwo)
            {
                periodToWait -= 4;
                hitQuartileTwo = true;
            }
            else if (currentKeyPress <= keyPressQuartileThree && !hitQuartileThree)
            {
                // The same process is done again for the third quartile,
                // but this time, only decrease the delay by 2 cycles...
                periodToWait -= 2;
                hitQuartileThree = true;
            }

            // Wait the period to wait, which could start at 28,
            // or decrease based on the numbers in the conditionals
            // written above
            wait(periodToWait);
        }
    }
}