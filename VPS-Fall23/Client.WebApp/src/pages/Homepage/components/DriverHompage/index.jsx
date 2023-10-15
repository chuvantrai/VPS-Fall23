import { Fragment, useEffect, useMemo, useState } from 'react';
import FoundedParkingZone from './FoundedParkingZone';
import { useSelector } from 'react-redux';
const { Map } = await google.maps.importLibrary('maps');
const { AdvancedMarkerElement } = await google.maps.importLibrary('marker');
let map;
async function initMap(focusPosition) {
  // The location of Uluru
  const position = focusPosition;
  // Request needed libraries.
  //@ts-ignore

  // The map, centered at Uluru
  map = new Map(document.getElementById('map'), {
    zoom: 12,
    center: position,
    mapId: 'DEMO_MAP_ID',
  });
}

const DriverHompage = () => {
  const [focusPosition, setFocusPosition] = useState({ lat: 20.98257, lng: 105.844949 });
  const { listFounded } = useSelector((state) => state.parkingZone);
  useEffect(() => {
    initMap(focusPosition);
    listFounded.map((parkingZone, index) => {
      const marker = new AdvancedMarkerElement({
        map: map,
        position: {
          lat: parkingZone.lat,
          lng: parkingZone.lng,
        },
        title: parkingZone.name,
      });
      const infoWindow = new google.maps.InfoWindow({
        content: parkingZone.name,
        disableAutoPan: false,
      });
      marker.addListener('click', () => {
        setDetailFormInfo({ parkingZone: parkingZone, isShow: true });
      });
    });
  }, [JSON.stringify(focusPosition)]);
  useMemo(() => {
    if (listFounded.length === 0) return;
    setFocusPosition({ lat: listFounded[0].lat, lng: listFounded[0].lng });
  }, [JSON.stringify(listFounded)]);
  const viewOnThisMapCallback = (parkingZone) => {
    const marker = new AdvancedMarkerElement({
      map: map,
      position: {
        lat: parkingZone.lat,
        lng: parkingZone.lng,
      },
      title: parkingZone.name,
    });
    const infoWindow = new google.maps.InfoWindow({
      content: parkingZone.name,
      disableAutoPan: false,
    });
    infoWindow.open(map, marker);
  };
  return (
    <Fragment>
      <div
        id="map"
        style={{
          width: '100vw',
          maxWidth: '100%',
          position: 'absolute',
          height: '100%',
        }}
      ></div>
      <FoundedParkingZone viewOnThisMapCallback={viewOnThisMapCallback}></FoundedParkingZone>
    </Fragment>
  );
};
export default DriverHompage;
