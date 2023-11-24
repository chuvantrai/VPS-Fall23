import { Modal, Select, Space, notification } from "antd";
import { useCallback, useEffect, useMemo, useState } from "react";
import AddressCascader from '@/components/cascader/AddressCascader';
import { useDebounce } from "@uidotdev/usehooks";
import { v4 as uuidv4 } from "uuid";
import useGoongMapService from '@/services/goongMapServices';
import useParkingZoneService from '@/services/parkingZoneService';
let map, marker;

async function initMap(focusPosition) {
    // The location of Uluru
    const position = focusPosition;
    map = new goongjs.Map({
        container: 'pz-update-address-modal', // container id
        style: 'https://tiles.goong.io/assets/goong_map_web.json', // stylesheet location
        center: position, // starting position [lng, lat]
        zoom: 12 // starting zoom
    });
    marker = new goongjs.Marker({
        draggable: true
    })
        .setLngLat(focusPosition)
        .addTo(map);
}

const ParkingZoneAddressModal = ({ isShow, parkingZone, onCloseCallback }) => {

    //address cascader
    const [selectedAddress, setSelectedAddress] = useState([]);
    const [locationSearchValue, setLocationSearchValue] = useState(null)
    const locationSearchValueDebound = useDebounce(locationSearchValue, 600)
    const [sessionToken, setSessionToken] = useState(uuidv4())
    const [locationOptions, setLocationOptions] = useState([])
    const [selectedLocationDetail, setSelectedLocationDetail] = useState();
    const [pointedLocation, setPointedLocation] = useState({ lng: parkingZone.location.x, lat: parkingZone.location.y });
    const [allowUpdate, setAllowUpdate] = useState(false);

    const goongMapService = useGoongMapService();
    const parkingZoneService = useParkingZoneService();
    function onDragEnd() {
        var lngLat = marker.getLngLat();
        setPointedLocation({ lng: lngLat.lng, lat: lngLat.lat })
        setLocationOptions([])
    }

    const onLocationSearch = (value) => {
        const addressCombine = [value, ...selectedAddress.reverse().map((a) => a.name)].join(', ')
        if (!addressCombine) return;
        goongMapService.placeAutoComplete(addressCombine, sessionToken)
            .then(res => setLocationOptions(res.data))
    }

    const addressCascaderProps = {
        style: { width: '50%' },
        placeholder: 'Chọn địa chỉ',
    };
    const onCascaderChange = useCallback((value, selectedOptions) => {
        setSelectedAddress(selectedOptions ?? []);
    }, []);

    const onAddressSelected = (placeId) => {
        goongMapService
            .getPlaceDetail(placeId, sessionToken)
            .then(res => {
                map.jumpTo({ zoom: map.getZoom(), center: res.data.geometry.position })
                marker.setLngLat(res.data.geometry.position)
                setSelectedLocationDetail(res.data);
                setSessionToken(uuidv4())
                setAllowUpdate(true)
            })
    }
    useEffect(() => {
        initMap({ lng: parkingZone.location.x, lat: parkingZone.location.y })
        marker.on("dragend", onDragEnd)
        return () => {
            marker.remove()
            map.remove()
        }
    }, [])
    useMemo(() => {
        onLocationSearch(locationSearchValueDebound)
    }, [locationSearchValueDebound])
    useMemo(() => {
        if (!pointedLocation?.lng || !pointedLocation?.lat) return;
        goongMapService
            .getPlaceFromLocation(pointedLocation)
            .then(res => {
                setLocationOptions(res.data);
                setSelectedLocationDetail(res.data.find((l) => (l.description ?? l.formattedAddress) == parkingZone.detailAddress))
            });
    }, [JSON.stringify(pointedLocation)])

    const update = () => {
        const data = {
            parkingZoneId: parkingZone.key,
            detailAddress: selectedLocationDetail.formattedAddress,
            location: {
                lat: selectedLocationDetail.geometry.position.lat,
                lng: selectedLocationDetail.geometry.position.lng,
            }
        }
        parkingZoneService
            .updateParkingZoneAddress(data)
            .then(res => {
                notification.success({ description: "Cập nhật thành công" })
            })
    }
    return (
        <Modal
            open={isShow}
            width={"60vw"}
            destroyOnClose={true}
            onCancel={onCloseCallback}
            cancelText="Đóng"
            okText="Cập nhật"
            onOk={update}
            okButtonProps={{ disabled: !allowUpdate }}
        >
            <Space.Compact style={{ marginBottom: "10px", width: "50%" }} title="Nhập và chọn địa chỉ">

                <AddressCascader cascaderProps={addressCascaderProps} onCascaderChangeCallback={onCascaderChange} />
                <Select
                    showSearch
                    allowClear
                    filterOption={() => true}
                    options={locationOptions.map((p) => {
                        return {
                            value: p.placeId,
                            label: p.description ?? p.formattedAddress
                        }
                    })}
                    value={selectedLocationDetail?.placeId}
                    style={{ width: "50%" }}
                    placeholder="Địa chỉ cụ thể"
                    onSearch={setLocationSearchValue}
                    onSelect={onAddressSelected}
                    onClear={() => { setSessionToken(uuidv4()) }}
                />
            </Space.Compact>


            <div style={{ position: "relative", height: "60vh" }}
            >

                <div id="pz-update-address-modal" style={{ position: "absolute", left: 0, top: 0, width: "100%", height: "100%" }}>

                </div>
            </div>

        </Modal>
    )
}
export default ParkingZoneAddressModal