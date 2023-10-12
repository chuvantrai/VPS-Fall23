import { Fragment, useCallback, useEffect, useMemo, useState } from "react"
import FoundedParkingZone from "./FoundedParkingZone";
import { useSelector } from "react-redux";
import ParkingZoneDetail from "./ParkingZoneDetail";
import { notification } from "antd";
const { Map } = await google.maps.importLibrary("maps");
const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");
let map;
async function initMap(focusPosition) {
    // The location of Uluru
    const position = focusPosition;
    // Request needed libraries.
    //@ts-ignore


    // The map, centered at Uluru
    map = new Map(document.getElementById("map"), {
        zoom: 12,
        center: position,
        mapId: "DEMO_MAP_ID",
    });

    // The marker, positioned at Uluru
    const marker = new AdvancedMarkerElement({
        map: map,
        position: position,
        title: "Hóa đơn điện tử M-invoice",

    });
    const infoWindow = new google.maps.InfoWindow({
        content: "Hóa đơn điện tử M-invoice",
        disableAutoPan: true,
    });
    marker.addListener("click", () => {
        infoWindow.open(map, marker);
    });
}

const DriverHompage = () => {

    const [focusPosition, setFocusPosition] = useState({ lat: 20.982570, lng: 105.844949 })
    const { listFounded } = useSelector(state => state.parkingZone);
    const [detailFormInfo, setDetailFormInfo] = useState({ parkingZone: null, isShow: false });

    useEffect(() => {
        initMap(focusPosition);
        listFounded.map((parkingZone, index) => {
            const marker = new AdvancedMarkerElement({
                map: map,
                position: {
                    lat: parkingZone.lat,
                    lng: parkingZone.lng
                },
                title: parkingZone.name,
            });
            const infoWindow = new google.maps.InfoWindow({
                content: parkingZone.name,
                disableAutoPan: false,
            });
            marker.addListener("click", () => {
                setDetailFormInfo({ parkingZone: parkingZone, isShow: true })
            });
        })
    }, [JSON.stringify(focusPosition)])
    useMemo(() => {
        if (listFounded.length === 0) return
        setFocusPosition({ lat: listFounded[0].lat, lng: listFounded[0].lng })
    }, [JSON.stringify(listFounded)])
    const viewOnGoogleMapCallback = (parkingZone) => {
        window.open(`https://maps.google.com/?q=${parkingZone.lat},${parkingZone.lng}`, "_blank");
    }
    const viewOnThisMapCallback = (parkingZone) => {

    }
    const onDetailCloseCallback = useCallback(() => {
        setDetailFormInfo({ parkingZone: null, isShow: false })
    }, [])
    const bookingCallback = useCallback(() => {
        notification.success({ message: "Chức năng đang cập nhật" });
    }, [])
    return (
        < Fragment>
            <div id="map" style={{
                width: '100vw',
                maxWidth: '100%',
                position: "absolute",
                height: "100%"
            }}>

            </div>
            <FoundedParkingZone
                viewOnGoogleMapCallback={viewOnGoogleMapCallback}
                viewOnThisMapCallback={viewOnThisMapCallback}
            >

            </FoundedParkingZone>
            <ParkingZoneDetail
                {...detailFormInfo}
                onCloseCallback={onDetailCloseCallback}
                bookingCallback={bookingCallback}></ParkingZoneDetail>
        </Fragment >)
}
export default DriverHompage