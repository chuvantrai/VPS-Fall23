import React, { useEffect, useState, useCallback, Fragment } from 'react';
import useParkingZoneService from '@/services/parkingZoneService';
import { useDebounce } from 'use-debounce';
import ParkingZonesTable from './Table';
import { ViewParkingZoneContext } from '@/hooks/useContext/viewParkingZone.context';
import ParkingZonesHeader from './Header';
import UpdateAddressModal from './Detail/Address/UpdateAddressModal';
import UpdateParkingZoneModal from './Detail/Update/UpdateModal';
import ParkingZoneDescriptionModal from './Detail/Description/DescriptionModal';
import CloseParkingZoneModal from './Detail/Close/CloseModal';

const PAGE_SIZE = 10;
function ViewListParkingZone() {

  const parkingZoneService = useParkingZoneService();

  const [detailInfo, setDetailInfo] = useState({ isShow: false, parkingZone: null, type: '' });
  const [viewValues, setViewValues] = useState({
    reload: true,
    searchValue: null,
    currentPage: 1,
    totalItems: 0,
    pageSize: PAGE_SIZE,
    parkingZones: []
  })
  const contextValue = {
    detailInfo: detailInfo,
    setDetailInfo: setDetailInfo,
    viewValues: viewValues,
    setViewValues: setViewValues
  }
  const [searchValueDebounce] = useDebounce(viewValues.searchValue, 500);

  useEffect(() => {
    getData(viewValues.currentPage, viewValues.pageSize, searchValueDebounce);
    // return () => {
    //   setViewValues({ currentPage: 1, totalItems: 0, pageSize: PAGE_SIZE, searchValue: null, parkingZones: [] });
    //   setDetailInfo({ isShow: false, parkingZone: null })
    // }
  }, [viewValues.currentPage, viewValues.pageSize, searchValueDebounce]);
  useEffect(() => {
    if (viewValues.reload === true) {
      getData(viewValues.currentPage, viewValues.pageSize, searchValueDebounce);
      return;
    }
  }, [viewValues.reload])
  const getData = (currentPage, pageSize, searchValue) => {
    const queryData = { pageNumber: currentPage, pageSize: pageSize, name: searchValue }
    const functionName = searchValue ? "getParkingZoneByName" : "getAllParkingZone"
    parkingZoneService[functionName](queryData)
      .then((res) => {
        setViewValues({
          ...viewValues,
          parkingZones: res.data.data.map((pz) => ({ ...pz, key: pz.id })),
          totalItems: res.data.totalCount,
          reload: false
        });
      })
  };

  const getDetailModal = () => {
    if (!detailInfo.parkingZone) return <></>
    switch (detailInfo.type) {
      case 'address': {
        return <UpdateAddressModal />
      }
      case 'update': {
        return detailInfo.parkingZone?.isApprove === true ?
          <ParkingZoneDescriptionModal /> : <UpdateParkingZoneModal />
      }
      case 'close': {
        return <CloseParkingZoneModal />
      }
      default: return <></>
    }
  }

  return (
    <ViewParkingZoneContext.Provider
      value={contextValue} >
      <div className="w-full px-4">
        <ParkingZonesHeader />
        <ParkingZonesTable parkingZones={viewValues.parkingZones} />
      </div>
      {
        getDetailModal()
      }
    </ViewParkingZoneContext.Provider>
  );
}

export default ViewListParkingZone;
