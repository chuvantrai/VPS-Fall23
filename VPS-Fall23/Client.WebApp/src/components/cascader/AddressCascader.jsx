import { useEffect, useMemo, useState } from "react"
import useAddressServices from '@/services/addressServices';
import { Cascader } from 'antd';
const getDataTypes = {
  city: {
    getFuncName: 'getDistricts',
    childType: 'district',
    isLeaf: false,
  },
  district: {
    getFuncName: 'getCommunes',
    childType: 'commune',
    isLeaf: true,
  },
};
const fieldNames = {
  label: 'name',
  value: 'id',
  children: 'children',
};

const AddressCascader = ({ cascaderProps, onCascaderChangeCallback, defaultAddress }) => {
  const [cities, setCities] = useState([]);
  const addressService = useAddressServices();
  const [defaultAddressId, setDefaultAddressId] = useState(defaultAddress ? [defaultAddress] : undefined);
  useEffect(() => {
    addressService
      .getCities()
      .then((res) => {
        const data = res.data.map((val) => ({
          ...val,
          isLeaf: false,
          type: 'city',
        }))
        setCities(data);
      })
  }, []);
  useEffect(() => {
    if (cities.length == 0) return;
    loadDefaultValue()
    return () => {
      setCities([])
    }
  }, [cities.length])
  const loadDefaultValue = () => {
    if (!defaultAddress) return;
    addressService.findAddressById(defaultAddress)
      .then(async (response) => {
        const data = response.data;
        const cityId = data.district?.city?.id ?? data.city?.id ?? defaultAddress
        const city = cities.find(city => city.id === cityId)
        await loadCascaderChildren([city]);
        const districtId = data.district?.id ?? data.id ?? (cityId === defaultAddress ? null : defaultAddress);
        if (districtId) {
          const district = city.children.find(district => district.id === districtId);
          await loadCascaderChildren([city, district])
        }
        const communeId = (defaultAddress === cityId || defaultAddress === districtId) ? null : data.id;
        setDefaultAddressId([cityId, districtId, communeId])


      });
  }
  const getChildCallback = (targetOption, childData, type, isLeaf) => {
    targetOption.children = childData.map((val) => ({ ...val, type: type, isLeaf: isLeaf }));
    setCities([...cities]);
  };
  const loadCascaderChildren = async (selectedOptions) => {
    console.log(selectedOptions);
    const targetOption = selectedOptions[selectedOptions.length - 1];
    if (targetOption.children && targetOption.children.length > 0) {
      return;
    }
    let getType = getDataTypes[targetOption.type];
    await addressService[getType.getFuncName](targetOption.id).then((res) =>
      getChildCallback(targetOption, res.data, getType.childType, getType.isLeaf),
    );
  };
  return (
    <Cascader
      {...cascaderProps}
      options={cities}
      defaultValue={defaultAddressId}
      loadData={loadCascaderChildren}
      onChange={onCascaderChangeCallback}
      fieldNames={fieldNames}
      changeOnSelect
      status=""
    />
  );
};
export default AddressCascader;
