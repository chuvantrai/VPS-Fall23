
import { Select, Space, notification } from 'antd';
import { useCallback, useEffect, useState } from 'react';
import { setSelectedLocation } from '@/stores/parkingZones/parkingZone.store';
import store from '@/stores/index';
import AddressCascader from '@/components/cascader/AddressCascader';
import { v4 as uuidv4 } from 'uuid'
import useGoongMapService from '@/services/goongMapServices';
import { useDebounce } from '@uidotdev/usehooks';

const DriverCenterHeader = () => {
    const [selectedAddress, setSelectedAddress] = useState([]);
    const [input, setInput] = useState("");
    const [sessionToken, setSessionToken] = useState()
    const [placeAutoCompleteData, setPlaceAutoCompleteData] = useState([]);
    const [selectedPlace, setSelectedPlace] = useState(null);

    const goongMapService = useGoongMapService();
    const inputDebouneValue = useDebounce(input, 600);
    useEffect(() => {
        setPlaceAutoCompleteData([])
        if (!inputDebouneValue) return;
        onSearch(inputDebouneValue);

    }, [inputDebouneValue])

    const addressCascaderProps = {
        style: { width: '40%' },
        placeholder: 'Chọn địa điểm',

    };
    useEffect(() => {
        onSearch(inputDebouneValue);
    }, [JSON.stringify(selectedAddress)])

    useEffect(() => {
        setSessionToken(uuidv4())
    }, [])
    useEffect(() => {
        if (Array.isArray(placeAutoCompleteData) && placeAutoCompleteData.length > 0) {
            setSelectedPlace(placeAutoCompleteData[0]);
        }
    }, [JSON.stringify(placeAutoCompleteData)])
    useEffect(() => {
        onPlaceSelect(selectedPlace?.placeId)
    }, [JSON.stringify(selectedPlace)])
    const onCascaderChange = useCallback((value, selectedOptions) => {
        setSelectedAddress(selectedOptions ?? []);
    }, []);
    const onSearch = (input) => {
        let inputCombine = [input, ...JSON.parse(JSON.stringify(selectedAddress)).reverse().map((address) => {
            return address.name
        })].join(', ')
        if (!inputCombine.trim()) {
            notification.error({
                message: "Vui lòng chọn hoặc nhập địa chỉ cần đến",
                description: "Vui lòng chọn hoặc nhập địa chỉ"
            })
            return;
        }

        goongMapService.placeAutoComplete(inputCombine, sessionToken)
            .then(res => setPlaceAutoCompleteData(res.data))

    };
    const onPlaceSelect = (value) => {
        goongMapService
            .getPlaceDetail(value, sessionToken)
            .then(res => {
                store.dispatch(setSelectedLocation({ selectedLocation: res?.data }));
                setSessionToken(uuidv4())
            })
    }

    return (
        <Space.Compact style={{ width: "100%" }}>
            <AddressCascader cascaderProps={addressCascaderProps} onCascaderChangeCallback={onCascaderChange} />
            <Select
                showSearch
                options={placeAutoCompleteData.map((p) => ({ label: p.description ?? p.formattedAddress, value: p.placeId }))}
                style={{ width: "60%" }}
                dropdownAlign={'end'}
                onSearch={(value) => { setInput(value); return true; }}
                onSelect={(value, option) => setSelectedPlace({ placeId: option.value, description: option.label })}
                value={selectedPlace?.placeId}
                onClear={() => { setSessionToken(uuidv4()) }}
                filterOption={() => true}
                allowClear
                placeholder="Nhập địa điểm chi tiết"
            >
            </Select>




        </Space.Compact>
    );
}
export default DriverCenterHeader