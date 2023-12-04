import { createContext, useContext } from "react";

export const UpdateParkingZoneAddressContext = createContext({
    goongMap: {
        map: null,
        marker: null
    },
    location: {
        queryValues: {
            sessionToken: '',
            searchValue: ''
        },
        selectedDetail: null,
        options: []
    },
    setGoongMap: () => { },
    setLocation: () => { },
})

export const useUpdateParkingZoneAddressContext = () => useContext(UpdateParkingZoneAddressContext)