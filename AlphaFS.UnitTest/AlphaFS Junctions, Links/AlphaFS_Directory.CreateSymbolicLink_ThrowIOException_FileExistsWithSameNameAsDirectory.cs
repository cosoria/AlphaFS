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
   public partial class AlphaFS_JunctionsLinksTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_CreateSymbolicLink_ThrowIOException_FileExistsWithSameNameAsDirectory_LocalAndNetwork_Success()
      {
         AlphaFS_Directory_CreateSymbolicLink_ThrowIOException_FileExistsWithSameNameAsDirectory(false);
         AlphaFS_Directory_CreateSymbolicLink_ThrowIOException_FileExistsWithSameNameAsDirectory(true);
      }

      
      private void AlphaFS_Directory_CreateSymbolicLink_ThrowIOException_FileExistsWithSameNameAsDirectory(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folderLink = System.IO.Path.Combine(tempRoot.Directory.FullName, "FolderLink-ToOriginalFolder");

            var fileInfo = new System.IO.FileInfo(System.IO.Path.Combine(tempRoot.Directory.FullName, "OriginalFile.txt"));
            using (fileInfo.Create()) {}

            Console.WriteLine("Input File Path     : [{0}]", fileInfo.FullName);
            Console.WriteLine("Input Directory Link: [{0}]", folderLink);

            ExceptionAssert.IOException(() => Alphaleonis.Win32.Filesystem.Directory.CreateSymbolicLink(folderLink, fileInfo.FullName));
         }

         Console.WriteLine();
      }
   }
}
