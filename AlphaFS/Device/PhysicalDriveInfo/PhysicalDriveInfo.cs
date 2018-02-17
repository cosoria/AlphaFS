/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Provides access to information of a physical drive.</summary>
   [Serializable]
   [SecurityCritical]
   public sealed class PhysicalDriveInfo
   {
      #region Constructors

      /// <summary>Initializes a PhysicalDriveInfo instance.</summary>
      private PhysicalDriveInfo()
      {
         StorageDeviceInfo = new StorageDeviceInfo
         {
            DeviceNumber = -1,
            PartitionNumber = -1
         };
      }


      /// <summary>Initializes a PhysicalDriveInfo instance.</summary>
      public PhysicalDriveInfo(StorageDeviceInfo storageInfo)
      {
         StorageDeviceInfo = storageInfo;
      }


      /// <summary>Initializes a PhysicalDriveInfo instance.</summary>
      public PhysicalDriveInfo(PhysicalDriveInfo physicalDriveInfo)
      {
         CopyTo(physicalDriveInfo, this);
      }

      #endregion // Constructors


      #region Properties

      /// <summary>The device description.</summary>
      public string DeviceDescription { get; internal set; }
      

      /// <summary>The path to the device.</summary>
      /// <returns>A string that represents the path to the device.
      ///   A drive path such as: "C:", "D:\",
      ///   a volume <see cref="Guid"/> path such as: "\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\"
      ///   or a <see cref="DeviceInfo.DevicePath"/> string.
      /// </returns>
      public string DevicePath { get; internal set; }


      /// <summary>The logical drives that are located on the physical drive.</summary>
      public Collection<string> LogicalDrives { get; internal set; }
      

      /// <summary>Indicates if the physical drive supports multiple outstanding commands (SCSI tagged queuing or equivalent). When false the physical drive does not support SCSI-tagged queuing or the equivalent.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Queueing")]
      public bool CommandQueueing { get; internal set; }


      /// <summary>Indicates if the physical drive is removable. When true the physical drive's media (if any) is removable. If the device has no media, this member should be ignored. When false the physical drive's media is not removable.</summary>
      public bool RemovableMedia { get; internal set; }


      /// <summary>The "FriendlyName" of the physical drive.</summary>
      public string Name { get; internal set; }

      
      /// <summary>The partition index numbers that are located on the physical drive.</summary>
      public Collection<int> PartitionIndexes { get; internal set; }


      ///// <summary>The partitions that are located on the physical drive.</summary>
      //public string[] Partitions { get; internal set; }

      /// <summary>The product revision of the physical drive.</summary>
      public string ProductRevision { get; internal set; }


      /// <summary>The storage device type and device- and partition number.</summary>
      public StorageDeviceInfo StorageDeviceInfo { get; internal set; }


      /// <summary>The serial number of the physical drive. If the physical drive has no serial number or the session is not elevated -1 is returned.</summary>
      public string SerialNumber { get; internal set; }


      /// <summary>The total size of the physical drive. If the session is not elevated -1 is returned</summary>
      public long TotalSize { get; internal set; }


      /// <summary>The total size of the physical drive, formatted as a unit size.</summary>
      public string TotalSizeUnitSize
      {
         get { return Utils.UnitSizeToText(TotalSize); }
      }


      /// <summary>A collection of volume GUID strings of volumes that are located on the physical drive.</summary>
      public Collection<string> VolumeGuids { get; internal set; }


      /// <summary>A collection of volume label strings of volumes that are located on the physical drive.</summary>
      public Collection<string> VolumeLabels { get; internal set; }

      #endregion // Properties


      #region Methods

      /// <summary>Checks if the logical drive or volume is located on the physical drive.</summary>
      /// <param name="devicePath">
      ///    A drive path such as: "C", "C:" or "C:\".
      ///    A volume <see cref="Guid"/> such as: "\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\".
      /// </param>
      public bool ContainsVolume(string devicePath)
      {
         bool isDrive;
         bool isVolume;
         bool isDeviceInfo;

         devicePath = Device.ValidateDevicePath(devicePath, out isDrive, out isVolume, out isDeviceInfo);


         if (isDrive && null != LogicalDrives)
         {
            devicePath = Path.RemoveTrailingDirectorySeparator(devicePath, false);

            return LogicalDrives.Any(driveName => driveName.Equals(devicePath, StringComparison.OrdinalIgnoreCase));
         }


         return isVolume && null != VolumeGuids && VolumeGuids.Any(guid => guid.Equals(devicePath, StringComparison.OrdinalIgnoreCase));
      }


      /// <summary>Returns the "FriendlyName" of the physical drive.</summary>
      /// <returns>A string that represents this instance.</returns>
      public override string ToString()
      {
         return Name ?? DevicePath;
      }


      /// <summary>Determines whether the specified Object is equal to the current Object.</summary>
      /// <param name="obj">Another object to compare to.</param>
      /// <returns><see langword="true"/> if the specified Object is equal to the current Object; otherwise, <see langword="false"/>.</returns>
      public override bool Equals(object obj)
      {
         if (null == obj || GetType() != obj.GetType())
            return false;

         var other = obj as PhysicalDriveInfo;

         return null != other && null != other.DevicePath && null != other.StorageDeviceInfo &&

                other.DevicePath.Equals(DevicePath, StringComparison.OrdinalIgnoreCase) &&

                other.StorageDeviceInfo.Equals(StorageDeviceInfo) &&

                other.StorageDeviceInfo.DeviceNumber.Equals(StorageDeviceInfo.DeviceNumber) && other.StorageDeviceInfo.PartitionNumber.Equals(StorageDeviceInfo.PartitionNumber);
      }


      /// <summary>Serves as a hash function for a particular type.</summary>
      /// <returns>A hash code for the current Object.</returns>
      public override int GetHashCode()
      {
         return null != DevicePath ? DevicePath.GetHashCode() : 0;
      }


      /// <summary>Implements the operator ==</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator ==(PhysicalDriveInfo left, PhysicalDriveInfo right)
      {
         return ReferenceEquals(left, null) && ReferenceEquals(right, null) || !ReferenceEquals(left, null) && !ReferenceEquals(right, null) && left.Equals(right);
      }


      /// <summary>Implements the operator !=</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator !=(PhysicalDriveInfo left, PhysicalDriveInfo right)
      {
         return !(left == right);
      }


      private static void CopyTo<T>(T source, T destination)
      {
         // Properties listed here should not be overwritten by the physical drive template.

         //var excludedProps = new[] {"PartitionNumber"};


         //var srcProps = typeof(T).GetProperties().Where(x => x.CanRead && x.CanWrite && !excludedProps.Any(prop => prop.Equals(x.Name))).ToArray();

         var srcProps = typeof(T).GetProperties().Where(x => x.CanRead && x.CanWrite).ToArray();

         var dstProps = srcProps.ToArray();


         foreach (var srcProp in srcProps)
         {
            var dstProp = dstProps.First(x => x.Name.Equals(srcProp.Name));

            dstProp.SetValue(destination, srcProp.GetValue(source, null), null);
         }
      }

      #endregion // Methods
   }
}