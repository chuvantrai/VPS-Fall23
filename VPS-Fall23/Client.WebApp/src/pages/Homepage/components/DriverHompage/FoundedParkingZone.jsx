import { useSelector } from 'react-redux';
import { useCallback, useEffect, useState } from 'react';
import ParkingZoneDetail from './ParkingZoneDetail/index';
import useParkingZoneService from '@/services/parkingZoneService.js';
import FoundedParkingZonePopover from './Popover/FoundedParkingZonePopover';
import BookmarkParkingZonePopover from './Popover/BookmarkParkingZonePopover';
import { ParkingZoneDetailContext } from '@/hooks/useContext/driver.parkingZoneDetail.context';

let markerList = []
const FoundedParkingZone = ({ map }) => {
  const { selectedLocation } = useSelector((state) => state.parkingZone);
  const [parkingZones, setParkingZones] = useState([]);
  const parkingZoneService = useParkingZoneService();


  useEffect(() => {
    if (!selectedLocation) return;
    const position = { lng: selectedLocation.geometry.position.lng, lat: selectedLocation.geometry.position.lat }
    parkingZoneService.getParkingZoneNearAround(position)
      .then(res => {
        setParkingZones(res.data);
        res.data.map((parkingZone) => {
          if (!parkingZone.location?.x || !parkingZone.location?.y) {
            return;
          }
          const location = { lng: parkingZone.location.x, lat: parkingZone.location.y }
          const popup = new goongjs.Popup()
            .setLngLat(location)
            .setHTML(`<h1>${parkingZone.name}</h1>`)
            .setMaxWidth("300px")
            .addTo(map);
          popup.on('click', () => {
            viewDetailInfo(parkingZone)
          })
          markerList.push(
            new goongjs.Marker()
              .setLngLat(location)
              .setPopup(popup)
              .addTo(map)
          );
        });
      })
    return () => {
      markerList.map((mk) => mk.remove())
      markerList = []
    }
  }, [JSON.stringify(selectedLocation)])


  const [detailFormInfo, setDetailFormInfo] = useState({ tab: '1', parkingZone: null, isShow: false });

  const viewDetailInfo = (parkingZone, tab = '1') => {
    setDetailFormInfo({ tab: tab, parkingZone: parkingZone, isShow: true })
  };
  const onDetailCloseCallback = useCallback(() => {
    setDetailFormInfo({ tab: '1', parkingZone: null, isShow: false });
  }, []);
  return (
    <ParkingZoneDetailContext.Provider value={{ detailFormInfo: detailFormInfo, setDetailFormInfo: setDetailFormInfo }} >
      <div className={'flex'}>
        <FoundedParkingZonePopover parkingZones={parkingZones} />
        <BookmarkParkingZonePopover />
      </div>
      {detailFormInfo.isShow === true && <ParkingZoneDetail
        isShow={detailFormInfo.isShow}
        parkingZone={detailFormInfo.parkingZone}
        onCloseCallback={onDetailCloseCallback}
      ></ParkingZoneDetail>}
    </ParkingZoneDetailContext.Provider>

  );
};
export default FoundedParkingZone;
