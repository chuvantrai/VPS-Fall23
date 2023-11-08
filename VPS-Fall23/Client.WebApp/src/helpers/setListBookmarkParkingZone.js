import keyNameLocalStorage from './keyNameLocalStorage.js';


const setListBookmarkParkingZone = (bookmarkArray) => {
  const bookmarkArrayJSON = JSON.stringify(Array.from(new Set(bookmarkArray)));
  localStorage.setItem(keyNameLocalStorage.LIST_BOOKMARK_PARKING_ZONE, bookmarkArrayJSON);
  
  if (bookmarkArrayJSON !== undefined && bookmarkArrayJSON !== '' && bookmarkArrayJSON !== null) {
    return JSON.parse(bookmarkArrayJSON);
  } else {
    return null;
  }
};

export default setListBookmarkParkingZone;
