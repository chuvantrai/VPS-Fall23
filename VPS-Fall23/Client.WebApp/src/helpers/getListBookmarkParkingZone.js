import keyNameLocalStorage from './keyNameLocalStorage.js';

const getListBookmarkParkingZone = () => {
  const bookmarkArrayJSON = localStorage.getItem(keyNameLocalStorage.LIST_BOOKMARK_PARKING_ZONE);
  if (bookmarkArrayJSON !== undefined && bookmarkArrayJSON !== '' && bookmarkArrayJSON !== null) {
    return JSON.parse(bookmarkArrayJSON);
  } else {
    return null;
  }
};

export default getListBookmarkParkingZone;
