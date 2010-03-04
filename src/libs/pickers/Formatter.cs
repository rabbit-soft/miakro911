/* ===============================================================================
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
 * ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
 * PARTICULAR PURPOSE.
 * 
 * (C) MSI 2007. All Rights Reserved.
 *
 * Portions of this code have been ported from Palo Mraz's ColorPicker 
 * in VB.NET to C#. All these portions © 2003-2004 LaMarvin.
 * For more information, see http://www.codeproject.com/vb/net/a_colorpicker.asp
 * and http://www.codeproject.com/vb/net/colorpicker2_cp.asp
 *
 * For questions/comments, please contact me at msafderiqbal@hotmail.com.
 * ===============================================================================
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Pickers
{
    /// <summary>
    /// Provides methods for formatting string representations of enumrated values for display.
    /// </summary>
    public static class Formatter
    {
        /// <summary>
        /// Inserts a space character before each capital letter in the given string.
        /// </summary>
        /// <param name="s">The <see cref="System.String"/> to insert spaces in.</param>
        /// <returns>A <see cref="System.String"/> having spaces before each capital letter.</returns>
        public static string InsertSpaces(string s)
        {
            string r = "";
            for (int i = 0; i <= s.Length - 1; i++)
                r += (Char.IsUpper(s[i])) ? " " + s[i].ToString() : s[i].ToString();
            r = r.Trim();
            return r;
        }
    }
}
