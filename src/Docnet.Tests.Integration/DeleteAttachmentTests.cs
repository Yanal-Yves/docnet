using Docnet.Core.Models;
using Docnet.Tests.Integration.Utils;
using System;
using System.IO;
using Xunit;

namespace Docnet.Tests.Integration
{
  [Collection("Lib collection")]
  public sealed class DeleteAttachmentTests
  {
    private readonly LibFixture _fixture;

    public DeleteAttachmentTests(LibFixture fixture)
    {
      _fixture = fixture;
    }

    [Fact]
    public void DeleteAttachment_WhenCalledWithNullString_ShouldThrow()
    {
      Assert.Throws<ArgumentNullException>(() => _fixture.Lib.DeleteAttachment((string)null));
    }

    [Fact]
    public void DeleteAttachment_WhenCalledWithNullBytes_ShouldThrow()
    {
      Assert.Throws<ArgumentNullException>(() => _fixture.Lib.DeleteAttachment((byte[])null));
    }

    [Fact]
    public void DeleteAttachment_WhenCalledWithEmptyBytes_ShouldThrow()
    {
      Assert.Throws<ArgumentNullException>(() => _fixture.Lib.DeleteAttachment(Array.Empty<byte>()));
    }

    [Theory]
    [InlineData("Docs/simple_0.pdf", 0)]
    [InlineData("Docs/pdf_with_attachment.pdf", 1)]
    public void DeleteAttachment_WhenCalledWithBytes_ShouldDeleteAttachment(string file, int expectedCount)
    {
      var bytes = File.ReadAllBytes(file);
      using (var reader = _fixture.Lib.GetDocReader(bytes, new PageDimensions(10, 10)))
      {
        Assert.Equal(expectedCount, reader.GetAttachmentCount());
      }

      var deletedAttachmentBytes = _fixture.Lib.DeleteAttachment(bytes);

      using (var deletedAttachmentReader = _fixture.Lib.GetDocReader(deletedAttachmentBytes, new PageDimensions(10, 10)))
      {
        Assert.Equal(0, deletedAttachmentReader.GetAttachmentCount());
      }
    }

    [Theory]
    [InlineData("Docs/simple_0.pdf", 0)]
    [InlineData("Docs/pdf_with_attachment.pdf", 1)]
    public void DeleteAttachment_WhenCalledWithString_ShouldDeleteAttachment(string file, int expectedCount)
    {
      using (var reader = _fixture.Lib.GetDocReader(file, new PageDimensions(10, 10)))
      {
        Assert.Equal(expectedCount, reader.GetAttachmentCount());
      }

      var deletedAttachmentBytes = _fixture.Lib.DeleteAttachment(file);

      using (var deletedAttachmentFile = new TempFile(deletedAttachmentBytes))
      {
        using (var deletedAttachmentReader = _fixture.Lib.GetDocReader(deletedAttachmentFile.FilePath, new PageDimensions(10, 10)))
        {
          Assert.Equal(0, deletedAttachmentReader.GetAttachmentCount());
        }
      }
    }
  }
}