import { useEffect, useState } from "react";
import ParkingZoneDetailModal from "../Modal";
import UpdateAddressMap from "./Map";
import UpdateAddressSearchBar from "./SearchBar";
import { UpdateParkingZoneAddressContext } from "@/hooks/useContext/viewParkingZone.address.context";
import { v4 as uuidv4 } from "uuid";
import { useViewParkingZoneContext } from "@/hooks/useContext/viewParkingZone.context";
import useParkingZoneService from '@/services/parkingZoneService';
import useGoongMapService from '@/services/goongMapServices';
import { notification } from "antd";

const UpdateAddressModal = () => {
    const { detailInfo, setDetailInfo, viewValues, setViewValues } = useViewParkingZoneContext();

    const selectedDetailInitValue = {
        placeId: '',
        description: detailInfo.parkingZone.detailAddress,
        geometry: {
            position:
            {
                lng: detailInfo.parkingZone.location.x,
                lat: detailInfo.parkingZone.location.y
            }
        }
    }
    const [goongMap, setGoongMap] = useState({ map: null, marker: null });
    const [location, setLocation] = useState({
        selectedDetail: selectedDetailInitValue,
        options: [],
        queryValues: {
            sessionToken: uuidv4(),
            searchValue: ''
        }
    });

    const parkingZoneService = useParkingZoneService();
    const goongMapService = useGoongMapService();


    const contextValue = {
        goongMap: goongMap,
        location: location,
        setGoongMap: setGoongMap,
        setLocation: setLocation
    }
    function onDragEnd(marker) {
        let lngLat = marker.target.getLngLat();
        setLocation({
            ...location,
            selectedDetail: {
                ...location.selectedDetail,
                geometry: {
                    position: {
                        lng: lngLat.lng, lat: lngLat.lat
                    }
                }
            },
            options: [],
        })
    }

    useEffect(() => {
        let newMapInstance = new goongjs.Map({
            container: 'pz-update-address-modal', // container id
            style: 'https://tiles.goong.io/assets/goong_map_web.json', // stylesheet location
            center: { lat: detailInfo.parkingZone.location.y, lng: detailInfo.parkingZone.location.x }, // starting position [lng, lat]
            zoom: 12 // starting zoom
        })
        let markerInstance = new goongjs.Marker({ draggable: !detailInfo.parkingZone.isApprove })
            .setLngLat({ lat: detailInfo.parkingZone.location.y, lng: detailInfo.parkingZone.location.x })
            .addTo(newMapInstance)
        markerInstance.on('dragend', onDragEnd);
        setGoongMap({ map: newMapInstance, marker: markerInstance })
        return () => {
            goongMap.map?.remove();
            goongMap.marker?.remove();
        }
    }, [])

    useEffect(() => {
        if (!location.selectedDetail?.geometry?.position?.lng || !location.selectedDetail?.geometry?.position?.lat) return;

        goongMap.map?.jumpTo({ zoom: goongMap.map.getZoom(), center: location.selectedDetail.geometry.position })
        goongMap.marker?.setLngLat(location.selectedDetail.geometry.position)
        goongMapService
            .getPlaceFromLocation(location.selectedDetail?.geometry?.position)
            .then(res => {
                const newLocation = {
                    ...location,
                    options: res.data,
                    selectedDetail: res.data[0]
                }
                setLocation(newLocation);
            });
    }, [JSON.stringify(location.selectedDetail.geometry.position)])

    const onCloseModal = () => {
        setDetailInfo({ parkingZone: null, isShow: false, type: '' })
    }
    const onUpdate = () => {
        const data = {
            parkingZoneId: detailInfo.parkingZone.id,
            detailAddress: location.selectedDetail.formattedAddress,
            location: {
                lat: location.selectedDetail.geometry.position.lat,
                lng: location.selectedDetail.geometry.position.lng,
            }
        }
        parkingZoneService
            .updateParkingZoneAddress(data)
            .then(res => {
                notification.success({ description: "Cập nhật thành công" })
                setViewValues({ ...viewValues, reload: true })
            })
    }
    const modalProps = {
        title: "Cập nhật địa chỉ nhà xe",
        open: detailInfo.isShow,
        width: "60vw",
        destroyOnClose: true,
        onCancel: onCloseModal,
        cancelText: "Đóng",
        okText: "Cập nhật",
        onOk: onUpdate,
        okButtonProps: { hidden: detailInfo.parkingZone.isApprove == true, disabled: false }
    }
    const modalContent = (
        <>
            <UpdateAddressSearchBar />
            <UpdateAddressMap />
        </>
    )
    return (
        <UpdateParkingZoneAddressContext.Provider value={contextValue}>
            <ParkingZoneDetailModal
                parkingZone={detailInfo.parkingZone}
                modalProps={modalProps}
                modalContent={modalContent}
            />
        </UpdateParkingZoneAddressContext.Provider>
    )
}
export default UpdateAddressModal;