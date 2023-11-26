import { createContext, useContext } from "react";
const initialValue = {
    isShow: false,
    parkingZone: null,
    type: null,
    searchValue: null,
}
const viewValuesInitValue = {
    searchValue: null,
    currentPage: 1,
    totalItems: 0,
    pageSize: 10
}
export const ViewParkingZoneContext = createContext({
    detailInfo: initialValue,
    viewValues: viewValuesInitValue,
    setDetailInfo: () => { },
    setViewValues: () => { }
})

export const useViewParkingZoneContext = () => useContext(ViewParkingZoneContext)