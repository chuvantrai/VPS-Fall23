import { Select, Space } from "antd"
import AddressCascader from '@/components/cascader/AddressCascader';
import { useEffect, useState } from "react";
import { useUpdateParkingZoneAddressContext } from "@/hooks/useContext/viewParkingZone.address.context";
import { v4 as uuidv4 } from "uuid";
import { useDebounce } from "@uidotdev/usehooks";
import useGoongMapService from '@/services/goongMapServices';
import { useViewParkingZoneContext } from "@/hooks/useContext/viewParkingZone.context";

const UpdateAddressSearchBar = () => {
    const { detailInfo } = useViewParkingZoneContext();
    const { goongMap, location, setGoongMap, setLocation } = useUpdateParkingZoneAddressContext();
    const [cascaderSelected, setCascaderSelected] = useState([]);


    const goongMapService = useGoongMapService();

    const onCascaderChange = (value, selectedOptions) => {
        setCascaderSelected(selectedOptions ?? []);
    }

    const addressCascaderProps = {
        style: { width: '50%' },
        placeholder: 'Chọn địa chỉ',
        disabled: detailInfo.parkingZone.isApprove
    };

    const [locationSearchValue, setLocationSearchValue] = useState();
    const locationSearchValueDebound = useDebounce(locationSearchValue, 500);

    useEffect(() => {
        onSearch(locationSearchValueDebound);
    }, [locationSearchValueDebound])

    const onSearch = (searchValue) => {
        const searchValueCombine = [searchValue?.trim(), ...cascaderSelected.reverse().map(c => c.name)].join(', ')
        if (!searchValueCombine) return;
        goongMapService
            .placeAutoComplete(searchValueCombine, location.queryValues.sessionToken)
            .then(res => {
                const newLocation = {
                    ...location,
                    options: res.data ?? [],
                    queryValues: {
                        ...location.queryValues,
                        searchValue: locationSearchValueDebound,
                    }
                }
                setLocation(newLocation);
            })

    }
    const onAddressSelected = (placeId) => {
        goongMapService
            .getPlaceDetail(placeId, location.queryValues.sessionToken)
            .then(res => {
                goongMap.map.jumpTo({ zoom: goongMap.map.getZoom(), center: res.data.geometry.position })
                goongMap.marker.setLngLat(res.data.geometry.position)
                const newLocation = {
                    ...location,
                    selectedDetail: res.data,
                    queryValues: {
                        ...location.queryValues,
                        sessionToken: uuidv4(),
                    }
                }
                setLocation(newLocation);
            })
    }

    return (
        <Space.Compact style={{ marginBottom: "10px", width: "100%" }} title="Nhập và chọn địa chỉ">
            <AddressCascader
                cascaderProps={addressCascaderProps}
                onCascaderChangeCallback={onCascaderChange} />
            <Select
                showSearch
                allowClear
                filterOption={() => true}
                options={location.options.map((p) => {
                    return {
                        value: p.placeId,
                        label: p.description ?? p.formattedAddress
                    }
                })}
                disabled={detailInfo.parkingZone.isApprove}
                value={location.selectedDetail.placeId}
                style={{ width: "50%" }}
                placeholder="Địa chỉ cụ thể"
                onSearch={setLocationSearchValue}
                onSelect={onAddressSelected}
                onClear={() => { setSessionToken(uuidv4()) }}
            />
        </Space.Compact>)
}
export default UpdateAddressSearchBar