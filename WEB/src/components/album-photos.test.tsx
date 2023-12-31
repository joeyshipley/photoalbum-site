import { describe, it, expect } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import { defaultContextSettings, PhotoAlbumContext } from 'src/context/photo-album.context';
import AlbumPhotos from 'src/components/album-photos';

describe('AlbumPhotos', () => {

  it('Renders no photos when they are not present.', () => {
    render(
      <PhotoAlbumContext.Provider value={{ ...defaultContextSettings() }}>
        <AlbumPhotos />
      </PhotoAlbumContext.Provider>
    );
    const photoElements = screen.queryAllByTestId('album-photo');

    expect(photoElements.length).toBe(0);
  });

  it('Renders the photos when present', () => {
    render(
      <PhotoAlbumContext.Provider value={{ ...defaultContextSettings(), albumPhotos: [
          { id: 101, albumId: 1, title: 'the photo 1', thumbnailUrl: 'test-1.jpg' },
          { id: 202, albumId: 2, title: 'the photo 2', thumbnailUrl: 'test-2.jpg' },
        ]}}>
        <AlbumPhotos />
      </PhotoAlbumContext.Provider>
    );
    const photoElements = screen.queryAllByTestId('album-photo');

    expect(photoElements.length).toBe(2);
  });

  it('Calls choosePhoto with correct id when photo is selected.', () => {
    let selectedId = -1;

    render(
      <PhotoAlbumContext.Provider value={{
        ...defaultContextSettings(),
        albumPhotos: [
          { id: 101, albumId: 1, title: 'My Album', thumbnailUrl: 'test' } ,
          { id: 202, albumId: 1, title: 'My Album', thumbnailUrl: 'test' }
        ],
        choosePhoto: (value) => { selectedId = value }
      }}>
        <AlbumPhotos />
      </PhotoAlbumContext.Provider>
    );

    const photos = screen.getAllByTestId('album-photo');
    fireEvent.click(photos[0]);

    expect(selectedId).toBe(101);
  });

});