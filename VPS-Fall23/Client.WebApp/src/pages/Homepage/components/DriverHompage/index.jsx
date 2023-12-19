import { Fragment, useEffect, useState } from 'react';
import FoundedParkingZone from './FoundedParkingZone';
import { useSelector } from 'react-redux';

let map;
let centerMarker;
async function initMap(focusPosition) {
  map = new goongjs.Map({
    container: 'map', // container id
    style: 'https://tiles.goong.io/assets/goong_map_web.json', // stylesheet location
    center: focusPosition, // starting position [lng, lat]
    zoom: 13
  });
}
const defaultSelectedLocation = {
  geometry: {
    position: {
      lat: 20.98257,
      lng: 105.844949
    }
  }
}


const DriverHompage = () => {
  let { selectedLocation } = useSelector((state) => state.parkingZone);
  const getLocation = () => {
    if (!selectedLocation) {
      selectedLocation = defaultSelectedLocation
    }
    return { lng: selectedLocation.geometry.position.lng, lat: selectedLocation.geometry.position.lat }
  }
  useEffect(() => {
    initMap(getLocation())
    return () => {
      map.remove()
    }
  }, [])
  useEffect(() => {
    const position = getLocation();
    map.jumpTo({ zoom: map.getZoom(), center: position });
    const popup = new goongjs.Popup()
      .setLngLat(position)
      .setHTML(`<h1>${selectedLocation.name}</h1>`)
    centerMarker = new goongjs.Marker({ color: '#C70D0F' })
      .setLngLat(position)
      .setPopup(popup)
      .addTo(map);
    return (() => {
      if (centerMarker) centerMarker.remove();
    })

  }, [JSON.stringify(selectedLocation)])

  return (
    <Fragment>
      <div
        id='map'
        style={{
          width: '100vw',
          maxWidth: '100%',
          position: 'absolute',
          height: '100%',
        }}
      ></div>
      <FoundedParkingZone map={map}></FoundedParkingZone>
    </Fragment>
  );
};
export default DriverHompage;
