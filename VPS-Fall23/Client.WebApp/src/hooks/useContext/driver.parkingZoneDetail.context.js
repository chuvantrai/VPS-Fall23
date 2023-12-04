import { createContext, useContext } from "react";
const initialValue = { tab: '1', parkingZone: null, isShow: false }
export const ParkingZoneDetailContext = createContext({
    detailFormInfo: initialValue,
    setDetailFormInfo: () => { }
})

export const useParkingZoneDetailContext = () => useContext(ParkingZoneDetailContext)