using Application.Infrastructure.External;
using Application.Photos;
using Application.Photos.Viewer;
using Application.Photos.Viewer.RequestsResults;
using FluentAssertions;
using Moq;
using Tests.Infrastructure.TestBases;

namespace Tests.Application.Photos;

public class PhotoViewerServiceTests
    : UnitTestOf<PhotoViewerService>
{
    [Test]
    public async Task ViewForAlbum_WhenAllIsWell()
    {
        // Arrange
        var apiCallerMock = Mocker.GetMock<IApiCaller>();
        apiCallerMock
            .Setup(x => x.GetAsync<List<PhotoEntry>>(It.IsAny<string>()))
            .ReturnsAsync(new ApiCallerResponse<List<PhotoEntry>>
            {
                Model = new List<PhotoEntry> { new PhotoEntry { Id = 1001 } },
            });
        var request = new PhotoViewerCollectionRequest { AlbumId = 1 };

        // Act
        var result = await UnderTest.ViewForAlbum(request);

        // Assert
        result.Photos.Count.Should().Be(1);
    }
    
    [Test]
    public async Task ViewForAlbum_WhenRequestIsInvalid()
    {
        // Arrange
        var request = new PhotoViewerCollectionRequest { AlbumId = 0 };

        // Act
        var result = await UnderTest.ViewForAlbum(request);

        // Assert
        result.Photos.Count.Should().Be(0);
        result.Errors.Count.Should().Be(1);
        result.Errors.Any(x => x.Key == "INVALID_ALBUM_ID").Should().BeTrue("INVALID_ALBUM_ID error was not found.");
    }
    
    [Test]
    public async Task ViewForAlbum_WhenErrorReturnedFromApi()
    {
        // Arrange
        var apiCallerMock = Mocker.GetMock<IApiCaller>();
        apiCallerMock
            .Setup(x => x.GetAsync<List<PhotoEntry>>(It.IsAny<string>()))
            .ReturnsAsync(new ApiCallerResponse<List<PhotoEntry>>
            {
                Errors = new List<string> { "Nope!" }
            });
        var request = new PhotoViewerCollectionRequest { AlbumId = 1 };

        // Act
        var result = await UnderTest.ViewForAlbum(request);

        // Assert
        result.Photos.Count.Should().Be(0);
        result.Errors.Count.Should().Be(1);
        result.Errors.Any(x => x.Key == "API_FAILURE").Should().BeTrue("API_FAILURE error was not found.");
    }

    [Test]
    public async Task View_WhenAllIsWell()
    {
        // Arrange
        var photoId = 1001;
        var apiCallerMock = Mocker.GetMock<IApiCaller>();
        apiCallerMock
            .Setup(x => x.GetAsync<PhotoEntry>(It.IsAny<string>()))
            .ReturnsAsync(new ApiCallerResponse<PhotoEntry>
            {
                Model = new PhotoEntry { Id = photoId },
            });

        var request = new PhotoViewerRequest { PhotoId = photoId };

        // Act
        var result = await UnderTest.View(request);

        // Assert
        result.Photo.Id.Should().Be(photoId);
    }

    [Test]
    public async Task View_WhenRequestIsInvalid()
    {
        // Arrange
        var request = new PhotoViewerRequest { PhotoId = 0 };

        // Act
        var result = await UnderTest.View(request);

        // Assert
        result.Photo.Should().BeNull();
        result.Errors.Count.Should().Be(1);
        result.Errors.Any(x => x.Key == "INVALID_PHOTO_ID").Should().BeTrue("INVALID_PHOTO_ID error was not found.");
    }

    [Test]
    public async Task View_WhenErrorReturnedFromApi()
    {
        // Arrange
        var photoId = 1001;
        var apiCallerMock = Mocker.GetMock<IApiCaller>();
        apiCallerMock
            .Setup(x => x.GetAsync<PhotoEntry>(It.IsAny<string>()))
            .ReturnsAsync(new ApiCallerResponse<PhotoEntry>
            {
                Errors = new List<string> { "Nope!" }
            });

        var request = new PhotoViewerRequest { PhotoId = photoId };

        // Act
        var result = await UnderTest.View(request);

        // Assert
        result.Photo.Should().BeNull();
        result.Errors.Count.Should().Be(1);
        result.Errors.Any(x => x.Key == "API_FAILURE").Should().BeTrue("API_FAILURE error was not found.");
    }
}