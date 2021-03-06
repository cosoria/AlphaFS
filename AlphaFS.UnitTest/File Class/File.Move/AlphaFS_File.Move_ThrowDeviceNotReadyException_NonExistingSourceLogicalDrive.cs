/*  Copyright (C) 2008-2018 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy 
 *  of this software and associated documentation files (the "Software"), to deal 
 *  in the Software without restriction, including without limitation the rights 
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 *  copies of the Software, and to permit persons to whom the Software is 
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in 
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 *  THE SOFTWARE. 
 */

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class MoveTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_File_Move_ThrowDeviceNotReadyException_NonExistingSourceLogicalDrive_LocalAndNetwork_Success()
      {
         AlphaFS_File_Move_ThrowDeviceNotReadyException_NonExistingSourceLogicalDrive(false);
         AlphaFS_File_Move_ThrowDeviceNotReadyException_NonExistingSourceLogicalDrive(true);
      }


      private void AlphaFS_File_Move_ThrowDeviceNotReadyException_NonExistingSourceLogicalDrive(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var nonExistingDriveLetter = Alphaleonis.Win32.Filesystem.DriveInfo.GetFreeDriveLetter();

         var srcFolder = nonExistingDriveLetter + @":\NonExisting Source File";
         var dstFolder = UnitTestConstants.SysDrive + @"\NonExisting Destination File";

         if (isNetwork)
         {
            srcFolder = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(srcFolder);
            dstFolder = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(dstFolder);
         }

         Console.WriteLine("Src File Path: [{0}]", srcFolder);
         Console.WriteLine("Dst File Path: [{0}]", dstFolder);


         ExceptionAssert.FileNotFoundException(() => System.IO.File.Move(srcFolder, dstFolder));

         ExceptionAssert.DeviceNotReadyException(() => Alphaleonis.Win32.Filesystem.File.Move(srcFolder, dstFolder));

         Assert.IsFalse(System.IO.Directory.Exists(dstFolder), "The file exists, but is expected not to.");
         
         Console.WriteLine();
      }
   }
}
