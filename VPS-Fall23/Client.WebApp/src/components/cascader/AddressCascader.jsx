import { useEffect, useState } from "react"
import useAddressServices from '@/services/addressServices';
import { Cascader } from "antd";
const getDataTypes = {
    city: {
        getFuncName: 'getDistricts',
        childType: 'district',
        isLeaf: false
    },
    district: {
        getFuncName: 'getCommunes',
        childType: 'commune',
        isLeaf: true
    }
}
const fieldNames = {
    label: "name",
    value: "id",
    children: 'children'
}

const AddressCascader = ({ cascaderProps, onCascaderChangeCallback, defaultAddress }) => {
    const [cities, setCities] = useState([]);
    const addressService = useAddressServices();
    useEffect(() => {
        addressService.getCities().then(res => {
            setCities(res.data.map((val) => {
                return {
                    ...val,
                    isLeaf: false,
                    type: "city"
                }
            }))
        })
    }, [defaultAddress])
    const getChildCallback = (targetOption, childData, type, isLeaf) => {
        targetOption.children = childData.map((val) => ({ ...val, type: type, isLeaf: isLeaf }))
        setCities([...cities])
    }
    const loadCascaderChildren = (selectedOptions) => {
        const targetOption = selectedOptions[selectedOptions.length - 1];
        if (targetOption.children && targetOption.children.length > 0) {
            return
        }
        let getType = getDataTypes[targetOption.type]
        addressService[getType.getFuncName](targetOption.id).then(res => getChildCallback(targetOption, res.data, getType.childType, getType.isLeaf))
    }
    return (<Cascader
        {...cascaderProps}
        options={cities}
        defaultValue={defaultAddress}
        loadData={loadCascaderChildren}
        onChange={onCascaderChangeCallback}
        fieldNames={fieldNames}
        changeOnSelect
        status='' />)
}
export default AddressCascader