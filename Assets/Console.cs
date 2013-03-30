using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace vvIO
{
    /// <summary>
    /// Contains methods for console input/output.
    /// </summary>
    public class vvConsole
    {
        private static GameObject field;
        private static Field fieldScript;

        /// <summary>
        /// Set the parent of the current scene's console (if it exists).
        /// Consoles are not objects due to Unity's method-based GUI system, and therefore the parent
        /// will not have a vvConsole member. Rather, a valid parent must implement a command table
        /// and GUI function for the console window. It must then call this method to attach
        /// its member output stream to the console.
        /// </summary>
        /// <param name="f">The parent object</param>
        public static void setConsoleParent(Field f)
        {
            fieldScript = f;
        }

        /// <summary>
        /// Prints a message to the VirtualVEX console.
        /// Useful for debugging purposes when making new robots (since the Unity print function is unavailable
        /// outside the Unity editor) or for implementing new console commands.
        /// </summary>
        /// <param name="message">The message to be printed</param>
        public static void print(string message)
        {
            fieldScript.cOutBuf += message;
        }

        /// <summary>
        /// Prints a message to the VirtualVEX console.
        /// Useful for debugging purposes when making new robots (since the Unity print function is unavailable
        /// outside the Unity editor) or for implementing new console commands.
        /// </summary>
        /// <param name="message">The message to be printed</param>
        public static void print(float message)
        {
            fieldScript.cOutBuf += message;
        }

        /// <summary>
        /// Prints a message to the VirtualVEX console.
        /// Useful for debugging purposes when making new robots (since the Unity print function is unavailable
        /// outside the Unity editor) or for implementing new console commands.
        /// </summary>
        /// <param name="message">The message to be printed</param>
        public static void print(int message)
        {
            fieldScript.cOutBuf += message;
        }

        /// <summary>
        /// Prints a message to the VirtualVEX console immediately followed by a newline.
        /// Equivalent to calling <code>print(message)</code> immediately followed by <code>print("\n")</code>
        /// </summary>
        /// <param name="message">The message to be printed</param>
        public static void println(string message)
        {
            print(message + "\n");
        }

        /// <summary>
        /// Prints a message to the VirtualVEX console immediately followed by a newline.
        /// Equivalent to calling <code>print(message)</code> immediately followed by <code>print("\n")</code>
        /// </summary>
        /// <param name="message">The message to be printed</param>
        public static void println(float message)
        {
            print(message + "\n");
        }

        /// <summary>
        /// Prints a message to the VirtualVEX console immediately followed by a newline.
        /// Equivalent to calling <code>print(message)</code> immediately followed by <code>print("\n")</code>
        /// </summary>
        /// <param name="message">The message to be printed</param>
        public static void println(int message)
        {
            print(message + "\n");
        }

        /// <summary>
        /// Prints a newline to the VirtualVEX console.
        /// Equivalent to calling <code>print("\n")</code>
        /// </summary>
        public static void println()
        {
            print("\n");
        }

        /// <summary>
        /// Clears the console buffer
        /// </summary>
        public static void clear()
        {
            fieldScript.cOutBuf = "";
        }
    }
}
