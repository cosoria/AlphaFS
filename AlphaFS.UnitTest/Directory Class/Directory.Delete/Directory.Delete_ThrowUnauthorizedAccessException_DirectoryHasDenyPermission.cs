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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AlphaFS.UnitTest
{
   public partial class DeleteTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void Directory_Delete_ThrowUnauthorizedAccessException_DirectoryHasDenyPermission_LocalAndNetwork_Success()
      {
         Directory_Delete_ThrowUnauthorizedAccessException_DirectoryHasDenyPermission(false);
         Directory_Delete_ThrowUnauthorizedAccessException_DirectoryHasDenyPermission(true);
      }


      private void Directory_Delete_ThrowUnauthorizedAccessException_DirectoryHasDenyPermission(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(tempRoot.Directory.FullName, "Existing Source Folder"));

            Console.WriteLine("Input Directory Path: [{0}]", folder);


            // Set DENY permission for current user.
            UnitTestConstants.FolderDenyPermission(true, folder.FullName);
            

            try
            {
               ExceptionAssert.IOException(() => System.IO.Directory.Delete(folder.FullName));


               // 2018-05-29 BUG: Throws wrong Exception.
               ExceptionAssert.UnauthorizedAccessException(() => Alphaleonis.Win32.Filesystem.Directory.Delete(folder.FullName));
            }
            finally
            {
               // Remove DENY permission for current user.
               UnitTestConstants.FolderDenyPermission(false, folder.FullName);
            }
         }
         
         Console.WriteLine();
      }
   }
}
