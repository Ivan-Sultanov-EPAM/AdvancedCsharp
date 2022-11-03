using FileObserverTask1;
using Moq;
using System;
using Xunit;

namespace FileObserverTask1Tests
{
    public class FileSystemVisitor_Should_
    {
        private readonly Mock<IConsole> _consoleMock;
        private readonly Mock<IFileSystem> _fileSystemMock;
        private FileSystemVisitor _fileSystemVisitor;
        private static readonly string _path = "Path";
        private static readonly string NewLine = Environment.NewLine;
        private static readonly string SearchResult = $"Search in {_path}{NewLine}{NewLine}--------- Search Result -----------{NewLine}";
        private static readonly string SearchFinished = $"{NewLine}-------- Search Finished ------------{NewLine}";
        string[] files = new[] { "file1", "file2", "file3" };

        public FileSystemVisitor_Should_()
        {
            _consoleMock = new Mock<IConsole>();
            _fileSystemMock = new Mock<IFileSystem>();
            _fileSystemMock.Setup(f => f.GetFiles(It.IsAny<String>())).Returns(files);
        }

        [Fact]
        public void Show_All_Files()
        {
            _fileSystemVisitor = new FileSystemVisitor(_fileSystemMock.Object, _consoleMock.Object, _path);
            _fileSystemVisitor.Search();

            _consoleMock.Verify(c => c.Write(It.Is<String>(s => s == SearchResult)), Times.Once());
            _consoleMock.Verify(c => c.Write(It.Is<String>(s => s == "file1")), Times.Once);
            _consoleMock.Verify(c => c.Write(It.Is<String>(s => s == "file2")), Times.Once);
            _consoleMock.Verify(c => c.Write(It.Is<String>(s => s == "file3")), Times.Once);
            _consoleMock.Verify(c => c.Write(It.Is<String>(s => s == SearchFinished)), Times.Once());
        }

        [Fact]
        public void Filter_Files()
        {
            _fileSystemVisitor = new FileSystemVisitor(_fileSystemMock.Object, _consoleMock.Object, _path,
                x => x.Contains("file2"));

            _fileSystemVisitor.Search();

            _consoleMock.Verify(c => c.Write(It.Is<String>(s => s == SearchResult)), Times.Once);
            _consoleMock.Verify(c => c.Write(It.Is<String>(s => s == "file1")), Times.Never);
            _consoleMock.Verify(c => c.Write(It.Is<String>(s => s == "file2")), Times.Once);
            _consoleMock.Verify(c => c.Write(It.Is<String>(s => s == "file3")), Times.Never);
            _consoleMock.Verify(c => c.Write(It.Is<String>(s => s == SearchFinished)), Times.Once);
        }

        [Fact]
        public void Show_No_Results_If_Files_Not_Found()
        {
            _fileSystemVisitor = new FileSystemVisitor(_fileSystemMock.Object, _consoleMock.Object, _path,
                x => x.Contains("NotExist"));

            _fileSystemVisitor.Search();

            _consoleMock.Verify(c => c.Write(It.Is<String>(s => s == SearchResult)), Times.Once);
            _consoleMock.Verify(c => c.Write(It.Is<String>(s => s == "file1")), Times.Never);
            _consoleMock.Verify(c => c.Write(It.Is<String>(s => s == "file2")), Times.Never);
            _consoleMock.Verify(c => c.Write(It.Is<String>(s => s == "file3")), Times.Never);
            _consoleMock.Verify(c => c.Write(It.Is<String>(s => s == "\t    No results")), Times.Once);
            _consoleMock.Verify(c => c.Write(It.Is<String>(s => s == SearchFinished)), Times.Once);
        }

        [Fact]
        public void Show_Folder()
        {
            _fileSystemMock.SetupSequence(f => f.GetDirectories(It.IsAny<String>()))
                .Returns(new[] { "Folder" })
                .Returns(new string[] { });

            _fileSystemMock.Setup(f => f.GetFiles(It.IsAny<String>())).Returns(new string[] { });

            _fileSystemVisitor = new FileSystemVisitor(_fileSystemMock.Object, _consoleMock.Object, _path);

            _fileSystemVisitor.Search();

            _consoleMock.Verify(c => c.Write(It.Is<String>(s => s == "[Folder]")), Times.Once);
        }
    }
}
