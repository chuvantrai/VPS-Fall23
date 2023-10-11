import { Button, Cascader, Col, Row } from 'antd';
import styles from './HeaderCenter.module.scss';
import classNames from 'classnames/bind';
import { useEffect, useState } from 'react';
import useAddressServices from '@/services/addressServices';

const cx = classNames.bind(styles);
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


const HeaderCenter = () => {

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
    }, [])
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
    const onCascaderChange = (value, selectedOptions) => {
        //  console.log(selectedOptions);
    };
    return (<div className={cx('wrapper')}>
        <Row gutter={2}>
            <Col md={20} sm={24}>
                <Cascader
                    style={{ width: "100%" }}
                    placeholder="Chọn địa điểm bạn muốn đến"
                    options={cities}
                    loadData={loadCascaderChildren}
                    onChange={onCascaderChange}
                    fieldNames={fieldNames}
                    changeOnSelect />

            </Col>
            <Col md={4}>  <Button>Search</Button></Col>

        </Row>
    </div>)
}
export default HeaderCenter
