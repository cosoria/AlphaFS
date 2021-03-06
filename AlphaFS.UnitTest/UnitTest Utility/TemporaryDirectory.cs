﻿/*  Copyright (C) 2008-2018 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Globalization;
using System.Security.AccessControl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   /// <summary>Used to create a temporary directory that will be deleted once this instance is disposed.</summary>
   internal sealed class TemporaryDirectory : IDisposable
   {
      #region Constructors

      public TemporaryDirectory() : this(false) { }
      

      public TemporaryDirectory(bool isNetwork, string folderPrefix = null, string root = null)
      {
         if (Alphaleonis.Utils.IsNullOrWhiteSpace(folderPrefix))
            folderPrefix = "AlphaFS.TempRoot";

         if (Alphaleonis.Utils.IsNullOrWhiteSpace(root))
            root = TempPath;

         if (isNetwork)
            root = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(root);


         UnitTestConstants.PrintUnitTestHeader(isNetwork);


         do
         {
            Directory = new System.IO.DirectoryInfo(System.IO.Path.Combine(root, folderPrefix + "." + RandomString));

         } while (Directory.Exists);

         Directory.Create();
      }

      #endregion // Constructors


      #region Properties

      public System.IO.DirectoryInfo Directory { get; private set; }


      /// <summary>Returns a random directory name, such as: "Directory_wqáánmvh".</summary>
      public string RandomDirectoryName
      {
         get { return string.Format(CultureInfo.InvariantCulture, "Directory.{0}", RandomString); }
      }


      /// <summary>Returns the full path to a non-existing file with a random name, such as: "File_wqáánmvh.txt".</summary>
      public string RandomTxtFileName
      {
         get { return string.Format(CultureInfo.InvariantCulture, "File_{0}.txt", RandomString); }
      }


      /// <summary>Returns the full path to a non-existing directory with a random name, such as: "C:\Users\UserName\AppData\Local\Temp\AlphaFS.TempRoot.lpqdzf\Directory_wqáánmvh.z03".</summary>
      public string RandomDirectoryFullPath
      {
         get { return System.IO.Path.Combine(Directory.FullName, RandomDirectoryName); }
      }


      /// <summary>Returns the full path to a non-existing file with a random name, such as: "C:\Users\UserName\AppData\Local\Temp\AlphaFS.TempRoot.lpqdzf\File_wqáánmvh.txt".</summary>
      public string RandomTxtFileFullPath
      {
         get { return System.IO.Path.Combine(Directory.FullName, RandomTxtFileName); }
      }


      /// <summary>Returns the full path to a non-existing file with a random name and without an extension, such as: "C:\Users\UserName\AppData\Local\Temp\AlphaFS.TempRoot.lpqdzf\File_wqáánmvh".</summary>
      public string RandomFileNoExtensionFullPath
      {
         get { return System.IO.Path.Combine(Directory.FullName, System.IO.Path.GetFileNameWithoutExtension(RandomTxtFileName)); }
      }
      
      
      /// <summary>Returns a random string of 8 characters in length, possibly with diacritic characters.</summary>
      public string RandomString
      {
         get
         {
            var randomFileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName());

            switch (new Random(DateTime.UtcNow.Millisecond).Next(1, 3))
            {
               case 1:
                  return randomFileName.Replace("a", "ä").Replace("e", "ë").Replace("i", "ï").Replace("o", "ö").Replace("u", "ü");

               case 2:
                  return randomFileName.Replace("a", "á").Replace("e", "é").Replace("i", "í").Replace("o", "ó").Replace("u", "ú");

               case 3:
                  return randomFileName.Replace("a", "â").Replace("e", "ê").Replace("i", "î").Replace("o", "ô").Replace("u", "û");

               default:
                  return randomFileName;
            }
         }
      }
      
      #endregion // Properties


      #region Methods
      
      /// <summary>Returns a <see cref="System.IO.DirectoryInfo"/> instance to an existing directory.</summary>
      public System.IO.DirectoryInfo CreateDirectory()
      {
         return CreateDirectory(null);
      }


      /// <summary>Returns a <see cref="System.IO.DirectoryInfo"/> instance to an existing directory, possibly with read-only and/or hidden attributes set.</summary>
      public System.IO.DirectoryInfo CreateDirectoryRandomizedAttributes()
      {
         return CreateDirectory(null, true, true);
      }




      /// <summary>Returns a <see cref="System.IO.FileInfo"/> instance to an existing file.</summary>
      public System.IO.FileInfo CreateFile()
      {
         return CreateFile(null);
      }


      /// <summary>Returns a <see cref="System.IO.DirectoryInfo"/> instance to an existing file, possibly with read-only and/or hidden attributes set.</summary>
      public System.IO.FileInfo CreateFileRandomizedAttributes()
      {
         return CreateFile(null, true, true);
      }




      /// <summary>Creates a directory structure of <param name="level"/> levels deep, populated with subdirectories and files with of random size.</summary>
      public System.IO.DirectoryInfo CreateTree(int level = 1)
      {
         return CreateTreeCore(null, level, false, false, false);
      }


      /// <summary>Creates a recursive directory structure of <param name="level"/> levels deep, populated with subdirectories and files with of random size.</summary>      
      public System.IO.DirectoryInfo CreateRecursiveTree(int level = 1)
      {
         return CreateTreeCore(null, level, true, false, false);
      }


      /// <summary>Creates a recursive directory structure of <param name="level"/> levels deep, populated with subdirectories and files with of random size.</summary>
      public System.IO.DirectoryInfo CreateRecursiveTree(int level, string rootFullPath)
      {
         return CreateTreeCore(rootFullPath, level, true, false, false);
      }


      /// <summary>Creates a directory structure of <param name="level"/> levels deep, populated with subdirectories and files with of random size.</summary>
      public System.IO.DirectoryInfo CreateRandomizedAttributesTree(int level = 1)
      {
         return CreateTreeCore(null, level, false, true, true);
      }


      /// <summary>Creates a recursive directory structure of <param name="level"/> levels deep, populated with subdirectories and files with of random size.</summary>
      public System.IO.DirectoryInfo CreateRecursiveRandomizedAttributesTree(int level = 1)
      {
         return CreateTreeCore(null, level, true, true, true);
      }
      
      
      /// <summary>Creates an, optional recursive, directory structure of <param name="level"/> levels deep, populated with subdirectories and files with of random size and possibly with read-only and/or hidden attributes set.</summary>
      private System.IO.DirectoryInfo CreateTreeCore(string rootFullPath, int level, bool recurse, bool readOnly, bool hidden)
      {
         var dirInfo = CreateDirectory(rootFullPath, readOnly, hidden);

         var folderCount = 0;


         for (var fsoCount = 0; fsoCount < level; fsoCount++)
         {
            folderCount++;

            var fsoName = RandomString + "-" + fsoCount;

            // Always create folder.
            var di = CreateDirectory(System.IO.Path.Combine(dirInfo.FullName, string.Format(CultureInfo.InvariantCulture, "Directory_{0}_directory", fsoName)), readOnly, hidden);

            // Create file, every other iteration.
            CreateFile(System.IO.Path.Combine(fsoCount % 2 == 0 ? di.FullName : dirInfo.FullName, string.Format(CultureInfo.InvariantCulture, "File_{0}_file.txt", fsoName)), readOnly, hidden);
         }


         if (recurse)
         {
            foreach (var folder in System.IO.Directory.EnumerateDirectories(dirInfo.FullName))
               CreateTreeCore(folder, level, false, readOnly, hidden);
         }


         Assert.AreEqual(level, folderCount, "The number of folders does not equal the level folder-level, but is expected to.");

         return dirInfo;
      }


      /// <summary> Enables or disables deny access for the current User.</summary>
      public void SetDirectoryDenyPermission(bool enable, string folderFullPath)
      {
         // ╔═════════════╦═════════════╦═══════════════════════════════╦════════════════════════╦══════════════════╦═══════════════════════╦═════════════╦═════════════╗
         // ║             ║ folder only ║ folder, sub-folders and files ║ folder and sub-folders ║ folder and files ║ sub-folders and files ║ sub-folders ║    files    ║
         // ╠═════════════╬═════════════╬═══════════════════════════════╬════════════════════════╬══════════════════╬═══════════════════════╬═════════════╬═════════════╣
         // ║ Propagation ║ none        ║ none                          ║ none                   ║ none             ║ InheritOnly           ║ InheritOnly ║ InheritOnly ║
         // ║ Inheritance ║ none        ║ Container|Object              ║ Container              ║ Object           ║ Container|Object      ║ Container   ║ Object      ║
         // ╚═════════════╩═════════════╩═══════════════════════════════╩════════════════════════╩══════════════════╩═══════════════════════╩═════════════╩═════════════╝

         var user = (Environment.UserDomainName + @"\" + Environment.UserName).TrimStart('\\');

         var rule = new FileSystemAccessRule(user, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Deny);

         DirectorySecurity dirSecurity;

         var dirInfo = CreateDirectory(folderFullPath);


         // Set DENY for current User.
         if (enable)
         {
            dirSecurity = dirInfo.GetAccessControl();
            dirSecurity.AddAccessRule(rule);
            dirInfo.SetAccessControl(dirSecurity);
         }

         // Remove DENY for current User.
         else
         {
            dirSecurity = dirInfo.GetAccessControl();
            dirSecurity.RemoveAccessRule(rule);
            dirInfo.SetAccessControl(dirSecurity);
         }
      }


      public override string ToString()
      {
         return Directory.FullName;
      }

      #endregion // Methods


      #region Private Members

      /// <summary>The path to the temporary folder, ending with a backslash.</summary>
      private static readonly string TempPath = System.IO.Path.GetTempPath();


      /// <summary>Returns a <see cref="System.IO.DirectoryInfo"/> instance to an existing directory, possibly with read-only and/or hidden attributes set.</summary>
      private System.IO.DirectoryInfo CreateDirectory(string folderFullPath, bool readOnly = false, bool hidden = false)
      {
         var dirInfo = System.IO.Directory.CreateDirectory(!Alphaleonis.Utils.IsNullOrWhiteSpace(folderFullPath) ? folderFullPath : RandomDirectoryFullPath);

         SetAttributes(dirInfo, readOnly, hidden);

         return dirInfo;
      }


      /// <summary>Returns a <see cref="System.IO.FileInfo"/> instance to an existing file, possibly with read-only and/or hidden attributes set.</summary>
      private System.IO.FileInfo CreateFile(string fileFullPath, bool readOnly = false, bool hidden = false)
      {
         var fileInfo = new System.IO.FileInfo(!Alphaleonis.Utils.IsNullOrWhiteSpace(fileFullPath) ? fileFullPath : RandomTxtFileFullPath);

         // File size is min 0 bytes, level 1 MB.
         using (var fs = fileInfo.Create())
            fs.SetLength(new Random(DateTime.UtcNow.Millisecond).Next(0, 1048576));

         SetAttributes(fileInfo, readOnly, hidden);

         return fileInfo;
      }


      private static void SetAttributes(System.IO.FileSystemInfo fsi, bool readOnly = false, bool hidden = false)
      {
         if (readOnly && new Random(DateTime.UtcNow.Millisecond).Next(0, 1000) % 2 == 0)
            fsi.Attributes |= System.IO.FileAttributes.ReadOnly;

         if (hidden && new Random(DateTime.UtcNow.Millisecond).Next(0, 1000) % 2 == 0)
            fsi.Attributes |= System.IO.FileAttributes.Hidden;
      }

      #endregion Private Members


      #region Disposable Members

      ~TemporaryDirectory()
      {
         Dispose(false);
      }


      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }


      private void Dispose(bool isDisposing)
      {
         try
         {
            if (isDisposing)
               System.IO.Directory.Delete(Directory.FullName, true);
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\nDelete TemporaryDirectory: [{0}]. Error: [{1}]", Directory.FullName, ex.Message.Replace(Environment.NewLine, string.Empty));
            Console.Write("Retry using AlphaFS... ");

            try
            {
               var dirInfo = new Alphaleonis.Win32.Filesystem.DirectoryInfo(Directory.FullName, Alphaleonis.Win32.Filesystem.PathFormat.FullPath);
               if (dirInfo.Exists)
               {
                  dirInfo.Delete(true, true);
                  Console.WriteLine("Success.");
               }

               else
                  Console.WriteLine("TemporaryDirectory was already removed.");
            }
            catch (Exception ex2)
            {
               Console.WriteLine("Delete failure TemporaryDirectory. Error: {0}", ex2.Message.Replace(Environment.NewLine, string.Empty));
            }
         }
      }

      #endregion // Disposable Members
   }
}
