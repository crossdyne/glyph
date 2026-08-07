using Glyph.Assets.Domain.ValueObjects.Assets;
using Glyph.Assets.Domain.ValueObjects.Categories;
using Glyph.Assets.Domain.ValueObjects.Projects;
using Glyph.Assets.Domain.ValueObjects.Shared;

namespace Glyph.Assets.Tests.Unit
{
    public class ValueObjectTests
    {
        #region Asset - AssetId

        [Fact]
        public void AssetId_From_ValidGuid_ReturnsAssetIdWithSameValue()
        {
            var guid = Guid.NewGuid();
            AssetId assetId = AssetId.From(guid);

            Assert.NotEqual(Guid.Empty, assetId.Value);
            Assert.Equal(guid, assetId.Value);
        }

        [Fact]
        public void AssetId_New_ReturnsNonEmptyAssetId()
        {
            AssetId assetId = AssetId.New();

            Assert.NotEqual(Guid.Empty, assetId.Value);
        }

        [Fact]
        public void AssetId_From_EmptyGuid_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => AssetId.From(Guid.Empty));
        }

        #endregion

        #region Asset - AssetName

        [Fact]
        public void AssetName_Create_ValidName_ReturnsAssetNameWithSameValue()
        {
            var name = "ValidName";
            AssetName assetName = AssetName.Create(name);

            Assert.Equal(name, assetName.Value);
        }

        [Fact]
        public void AssetName_Create_EmptyName_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => AssetName.Create(""));
        }

        #endregion

        #region Asset - AssetType

        [Fact]
        public void AssetType_FromName_ExistingName_ReturnsAssetTypeWithSameName()
        {
            string svg = AssetType.Svg.Name;
            AssetType assetType = AssetType.FromName(svg);

            Assert.Equal(svg, assetType.Name);
        }

        [Fact]
        public void AssetType_FromName_EmptyName_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => AssetType.FromName(""));
        }

        [Fact]
        public void AssetType_FormName_TrowsExceptionWhenValueNotFoundInCollection()
        {
            Assert.Throws<ArgumentException>(() => AssetType.FromName("Not_Exist_Value"));
        }
            
        [Fact]  
        public void AssetType_FormValue_ReturnValue()
        {
            int svg = AssetType.Svg.Value;
            AssetType assetType = AssetType.FromValue(svg);

            Assert.Equal(svg, assetType.Value);
        }

        [Fact]
        public void AssetType_FromValue_NonExistingValue_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => AssetType.FromValue(9999));
        }

        [Fact]
        public void AssetType_FromValue_NegativeValue_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => AssetType.FromValue(-999));
        }

        #endregion

        #region Asset - Format

        [Fact]
        public void Format_FromName_ReturnsValue()
        {
            string svg = Format.Svg.Name;
            Format format = Format.FromName(svg);

            Assert.Equal(svg, format.Name);
        }

        [Fact]
        public void Format_FromName_EmptyName_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Format.FromName(""));
        }

        [Fact]
        public void Format_FromName_NullName_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Format.FromName(null!));
        }

        [Fact]
        public void Format_FromName_NonExistingName_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Format.FromName("Not_Exist_value"));
        }

        [Fact]
        public void Format_FromValue_ReturnsValue()
        {
            int svg = Format.Svg.Value;
            Format format = Format.FromValue(svg);

            Assert.Equal(svg, format.Value);
        }

        [Fact]
        public void Format_FromValue_NonExistingValue_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Format.FromValue(999));
        }

        [Fact]
        public void Format_FromValue_ThrownExceptionWhenNameNotIncludeCollection()
        {
            Assert.Throws<ArgumentException>(() => Format.FromValue(65786));
        }

        [Fact]
        public void Format_TryFromName_ExistingName_ReturnsTrueAndAssetType()
        {
            string format = Format.Svg.Name;
            bool exist = Format.TryFromName(format, out var existingFormat);

            Assert.True(exist);
            Assert.Equal(Format.Svg, existingFormat);
        }
                
        [Fact]
        public void Format_TryFromName_NonExistingName_ReturnsFalseAndNull()
        {
            bool exist = Format.TryFromName("Not_Exist_Name", out var existingFormat);

            Assert.False(exist);
            Assert.NotEqual(Format.Svg, existingFormat);
        }

        #endregion

        #region Asset - MimeType

        [Fact]
        public void MineType_Create_CorrectValue_ReturnsMimeType()
        {
            string svgMimeType = MimeType.Svg.Value;
            MimeType mimeType = MimeType.Create(svgMimeType);

            Assert.NotEqual(default, mimeType);
            Assert.Equal(svgMimeType, mimeType.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void MimeType_Create_EmptyValue_ThrowArgumentException(string? value)
        {
            Assert.Throws<ArgumentException>(() => MimeType.Create(value!));
        }

        [Theory]
        [InlineData("Incorrect")]
        [InlineData("-format")]
        [InlineData("!format")]
        [InlineData("👩")]
        public void MimeType_Create_IncorrectValue_ThrowFormatException(string value)
        {
            Assert.Throws<FormatException>(() => MimeType.Create(value));
        }

        #endregion

        #region Asset - S3Key

        private const string Bucket = "TestBucket";
        private static readonly IReadOnlyCollection<string> Folders = ["user", "avatars"];
        private const string PathFolderString = "user/avatars"; 
        private const string FileName = "avatar.png";
        private const string CorrectStringS3KeyWithFolders = $"{Bucket}:{PathFolderString}/{FileName}";
        private const string CorrectStringS3KeyWithOutFolders = $"{Bucket}:{FileName}";

        [Theory]
        [InlineData(Bucket, new string[] { "user", "avatars" }, FileName)]
        [InlineData(Bucket, new string[] { }, FileName)]
        public void S3Key_Create_CorrectValues_ReturnS3Key(string bucket, string[] folders, string fileName)
        {
            S3Key s3Key = S3Key.Create(bucket, [.. folders], fileName);

            Assert.Equal(Bucket, s3Key.Bucket);
            Assert.True(Guid.TryParse(s3Key.FileName.Split('.').First(), out var _));
            
            if (folders.Count() > 0)
            {
                Assert.Equal(Folders, s3Key.Folders);
                Assert.Equal(PathFolderString, s3Key.FolderPath);
            }
            else
            {
                Assert.Empty(s3Key.Folders);
                Assert.Equal("", s3Key.FolderPath);
            }
        }

        [Theory]
        [InlineData("", new string[] { "user", "avatars" }, FileName)]
        [InlineData(Bucket, null, FileName)]
        [InlineData(Bucket, new string[] { "user", "avatars" }, "")]
        [InlineData(null, new string[] { "user", "avatars" }, FileName)]
        [InlineData(Bucket, new string[] { "user", "avatars" }, null)]
        public void S3Key_Create_EmptyValue_ThrowArgumentNullException(string? bucket, string[]? folders, string? fileName)
        {
            Assert.Throws<ArgumentException>(() => S3Key.Create(bucket!, folders!, fileName!));
        }

        [Fact]
        public void S3Key_Create_InvalidFileName_ThrowFormatException()
        {
            Assert.Throws<FormatException>(() => S3Key.Create(Bucket, [.. Folders], "fileName-without-extension"));
        }

        [Theory]
        [InlineData(CorrectStringS3KeyWithFolders)]
        [InlineData(CorrectStringS3KeyWithOutFolders)]
        public void S3Key_Restore_CorrectValues_ReturnS3Key(string s3KeyString)
        {
            S3Key s3Key = S3Key.Restore(s3KeyString);

            Assert.Equal(Bucket, s3Key.Bucket);
            Assert.Equal(s3KeyString, s3Key.Value);
            Assert.Equal(FileName.ToLowerInvariant(), s3Key.FileName);

            if (s3Key.Folders.Count() > 0)
                Assert.Equal(PathFolderString, s3Key.FolderPath);
        }

        [Theory]
        [InlineData("Incorrect")]
        [InlineData("/folders/user")]
        [InlineData("file.exe")]
        public void S3Key_Restore_IncorrectS3KeyString_ThrowFormatException(string value)
        {
            Assert.Throws<FormatException>(() => S3Key.Restore(value));
        }

        [Fact]
        public void S3Key_Restore_S3KeyStringWithOutSeparator_ThrowFormatException()
        {
            Assert.Throws<FormatException>(() => S3Key.Restore($"{Bucket}/{PathFolderString}/avatar.png"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void S3Key_Restore_S3KeyStringEmpty_ThrowArgumentException(string? value)
        {
            Assert.Throws<ArgumentException>(() => S3Key.Restore(value!));
        }

        [Fact]
        public void S3Key_GetObjectKey_WithFolders_ReturnCorrectValue()
        {
            S3Key s3Key = S3Key.Create(Bucket, [.. Folders], FileName);

            string fileName = s3Key.FileName;
            string objectKey = s3Key.GetObjectKey();

            Assert.Equal($"{PathFolderString}/{fileName}", objectKey);
        }

        [Fact]
        public void S3Key_GetObjectKey_WithOutFolders_ReturnCorrectValue()
        {
            S3Key s3Key = S3Key.Create(Bucket, [], FileName);

            string fileName = s3Key.FileName;
            string objectKey = s3Key.GetObjectKey();

            Assert.Equal(fileName, objectKey);
        }

        [Fact]
        public void S3Key_FolderPathProp_WithFolders_ReturnCorrectPath()
        {
            S3Key s3Key = S3Key.Create(Bucket, [.. Folders], FileName);

            Assert.Equal(PathFolderString, s3Key.FolderPath);
        }

        [Fact]
        public void S3Key_FolderPathProp_WithOutFolders_ReturnCorrectPath()
        {
            S3Key s3Key = S3Key.Create(Bucket, [], FileName);

            Assert.Equal("", s3Key.FolderPath);
        }

        #endregion

        #region Asset - SizeBytes

        [Fact]
        public void Create_ValidValue_ReturnsSizeBytesWithSameValue()
        {
            var result = SizeBytes.Create(555);

            Assert.Equal(555, result.Value);
        }

        [Fact]
        public void Create_Zero_ReturnsSizeBytesWithZero()
        {
            var result = SizeBytes.Create(0);

            Assert.Equal(0, result.Value);
        }

        [Fact]
        public void Create_MaxSize_ReturnsSizeBytesWithMaxSize()
        {
            var result = SizeBytes.Create(SizeBytes.MaxSize);

            Assert.Equal(SizeBytes.MaxSize, result.Value);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        [InlineData(SizeBytes.MaxSize + 1)]
        public void Create_OutOfRange_ThrowsArgumentOutOfRangeException(long value)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => SizeBytes.Create(value));
        }

        [Theory]
        [InlineData(1024L, 1.0)]
        [InlineData(2048L, 2.0)]
        [InlineData(512L, 0.5)]
        public void Kilobytes_ReturnsCorrectValue(long bytes, double expected)
        {
            SizeBytes size = bytes;

            Assert.Equal(expected, size.Kilobytes, precision: 5);
        }

        [Theory]
        [InlineData(1024L * 1024, 1.0)]
        [InlineData(1024L * 1024 * 5, 5.0)]
        public void Megabytes_ReturnsCorrectValue(long bytes, double expected)
        {
            SizeBytes size = bytes;

            Assert.Equal(expected, size.Megabytes, precision: 5);
        }

        [Theory]
        [InlineData(1024L * 1024 * 1024, 1.0)]
        [InlineData(1024L * 1024 * 1024 * 2, 2.0)]
        public void Gigabytes_ReturnsCorrectValue(long bytes, double expected)
        {
            SizeBytes size = bytes;

            Assert.Equal(expected, size.Gigabytes, precision: 5);
        }

        [Theory]
        [InlineData(0L, "0 B")]
        [InlineData(512L, "512 B")]
        [InlineData(1024L, "1 KB")]
        [InlineData(1024L * 1024, "1 MB")]
        [InlineData(1024L * 1024 * 1024, "1 GB")]
        [InlineData(1024L * 1024 * 1024 * 1024, "1 TB")]
        public void Humanity_ReturnsFormattedString(long bytes, string expected)
        {
            SizeBytes size = bytes;

            Assert.Equal(expected, size.Humanity());
        }

        [Fact]
        public void CompareTo_SizeBytes_Smaller_ReturnsNegative()
        {
            SizeBytes small = 100;
            SizeBytes large = 200;

            Assert.True(small.CompareTo(large) < 0);
        }

        [Fact]
        public void CompareTo_SizeBytes_Larger_ReturnsPositive()
        {
            SizeBytes small = 100;
            SizeBytes large = 200;

            Assert.True(large.CompareTo(small) > 0);
        }

        [Fact]
        public void CompareTo_SizeBytes_Equal_ReturnsZero()
        {
            SizeBytes a = 500;
            SizeBytes b = 500;

            Assert.Equal(0, a.CompareTo(b));
        }

        [Fact]
        public void CompareTo_Long_ReturnsSameAsUnderlyingValue()
        {
            SizeBytes size = 300;

            Assert.True(size.CompareTo(200L) > 0);
            Assert.True(size.CompareTo(400L) < 0);
            Assert.Equal(0, size.CompareTo(300L));
        }

        [Theory]
        [InlineData(100, 200, true)]
        [InlineData(200, 100, false)]
        [InlineData(100, 100, false)]
        public void LessThanOperator_ReturnsExpected(long left, long right, bool expected)
        {
            SizeBytes l = left;
            SizeBytes r = right;

            Assert.Equal(expected, l < r);
        }

        [Theory]
        [InlineData(200, 100, true)]
        [InlineData(100, 200, false)]
        [InlineData(100, 100, false)]
        public void GreaterThanOperator_ReturnsExpected(long left, long right, bool expected)
        {
            SizeBytes l = left;
            SizeBytes r = right;

            Assert.Equal(expected, l > r);
        }

        [Theory]
        [InlineData(100, 100, true)]
        [InlineData(100, 200, true)]
        [InlineData(200, 100, false)]
        public void LessThanOrEqualOperator_ReturnsExpected(long left, long right, bool expected)
        {
            SizeBytes l = left;
            SizeBytes r = right;

            Assert.Equal(expected, l <= r);
        }

        [Theory]
        [InlineData(100, 100, true)]
        [InlineData(200, 100, true)]
        [InlineData(100, 200, false)]
        public void GreaterThanOrEqualOperator_ReturnsExpected(long left, long right, bool expected)
        {
            SizeBytes l = left;
            SizeBytes r = right;

            Assert.Equal(expected, l >= r);
        }

        [Fact]
        public void ImplicitOperator_ToLong_ReturnsUnderlyingValue()
        {
            SizeBytes size = 1024;

            long result = size;

            Assert.Equal(1024L, result);
        }

        [Fact]
        public void ImplicitOperator_FromLong_CreatesSizeBytes()
        {
            SizeBytes size = 2048L;

            Assert.Equal(2048L, size.Value);
        }

        [Fact]
        public void Equals_SameValue_ReturnsTrue()
        {
            SizeBytes a = 100;
            SizeBytes b = 100;

            Assert.Equal(a, b);
        }

        [Fact]
        public void Equals_DifferentValue_ReturnsFalse()
        {
            SizeBytes a = 100;
            SizeBytes b = 200;

            Assert.NotEqual(a, b);
        }

        [Fact]
        public void ToString_ReturnsHumanityResult()
        {
            SizeBytes size = 1024 * 1024;

            Assert.Equal(size.Humanity(), size.ToString());
        }

        #endregion
   
        #region Category - CategoryId

        [Fact]
        public void CategoryId_From_ValidGuid_ReturnsCategoryIdWithSameValue()
        {
            var guid = Guid.NewGuid();
            CategoryId categoryId = CategoryId.From(guid);

            Assert.NotEqual(Guid.Empty, categoryId.Value);
            Assert.Equal(guid, categoryId.Value);
        }

        [Fact]
        public void CategoryId_New_ReturnsNonEmptyCategoryId()
        {
            CategoryId categoryId = CategoryId.New();

            Assert.NotEqual(Guid.Empty, categoryId.Value);
        }

        [Fact]
        public void CategoryId_From_EmptyGuid_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => CategoryId.From(Guid.Empty));
        }

        #endregion

        #region Category - CategoryName

        [Fact]
        public void CategoryName_Create_ValidName_ReturnsCategoryNameWithSameValue()
        {
            var name = "ValidName";
            CategoryName catName = CategoryName.Create(name);

            Assert.Equal(name, catName.Value);
        }

        [Fact]
        public void CategoryName_Create_EmptyName_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => CategoryName.Create(""));
        }

        #endregion

        #region Project - ProjectCode

        [Fact]
        public void ProjectCode_Create_ValidValue_ReturnsProjectCodeWithSameValue()
        {
            string testCode = "TEST_CODE";
            ProjectCode projectCode = ProjectCode.Create(testCode);

            Assert.Equal(testCode, projectCode.Value);
        }

        [Fact]
        public void ProjectCode_Create_EmptyGuid_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ProjectCode.Create(""));
        }

        #endregion
  
        #region Project - ProjectId

        [Fact]
        public void ProjectId_From_ValidGuid_ReturnsProjectIdWithSameValue()
        {
            var guid = Guid.NewGuid();
            ProjectId categoryId = ProjectId.From(guid);

            Assert.NotEqual(Guid.Empty, categoryId.Value);
            Assert.Equal(guid, categoryId.Value);
        }

        [Fact]
        public void ProjectId_New_ReturnsNonEmptyProjectId()
        {
            CategoryId categoryId = CategoryId.New();

            Assert.NotEqual(Guid.Empty, categoryId.Value);
        }

        [Fact]
        public void ProjectId_From_EmptyGuid_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => CategoryId.From(Guid.Empty));
        }

        #endregion
   
        #region Project - ProjectName

        [Fact]
        public void ProjectName_Create_ValidName_ReturnsProjectNameWithSameValue()
        {
            var name = "ValidName";
            ProjectName projectName = ProjectName.Create(name);

            Assert.Equal(name, projectName.Value);
        }

        [Fact]
        public void ProjectName_Create_EmptyName_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ProjectName.Create(""));
        }

        #endregion
   
        #region Shared - UserId

        [Fact]
        public void UserId_From_ValidGuid_ReturnsUserIdWithSameValue()
        {
            var guid = Guid.NewGuid();
            UserId categoryId = UserId.From(guid);

            Assert.NotEqual(Guid.Empty, categoryId.Value);
            Assert.Equal(guid, categoryId.Value);
        }

        [Fact]
        public void UserId_New_ReturnsNonEmptyUserId()
        {
            UserId categoryId = UserId.New();

            Assert.NotEqual(Guid.Empty, categoryId.Value);
        }

        [Fact]
        public void UserId_From_EmptyGuid_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => UserId.From(Guid.Empty));
        }

        #endregion
   }
}