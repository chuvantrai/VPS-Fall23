import { Button, Col, Form, Input, InputNumber, Modal, Row, Select, Space, TimePicker, Upload } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import { useState, useCallback, useEffect, useMemo } from 'react';
import { v4 as uuidv4 } from 'uuid';
import AddressCascader from '@/components/cascader/AddressCascader';
import useParkingZoneService from '@/services/parkingZoneService';
import { getAccountJwtModel } from '@/helpers';
import useGoongMapService from '@/services/goongMapServices';
import { useDebounce } from '@uidotdev/usehooks';

const layout = {
  labelCol: {
    span: 6,
  },
  wrapperCol: {
    span: 18,
  },
};

const validateMessages = {
  required: '${label} is required!',
  types: {
    email: '${label} is not a valid email!',
    number: '${label} is not a valid number!',
  },
  number: {
    range: '${label} must be between ${min} and ${max}',
  },
};

const getBase64 = (file) =>
  new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = (error) => reject(error);
  });

let map, marker;
async function initMap(focusPosition) {
  // The location of Uluru
  const position = focusPosition;
  map = new goongjs.Map({
    container: 'register-pz-map', // container id
    style: 'https://tiles.goong.io/assets/goong_map_web.json', // stylesheet location
    center: position, // starting position [lng, lat]
    zoom: 12 // starting zoom
  });
  marker = new goongjs.Marker({
    draggable: true
  })
    .setLngLat(defaulLocation.geometry.position)
    .addTo(map);
}
const defaulLocation = {
  geometry: {
    position: {
      lat: 20.98257,
      lng: 105.844949
    }
  }
}


const RegisterParkingZone = () => {
  const parkingZoneService = useParkingZoneService();
  const account = getAccountJwtModel();

  const [previewOpen, setPreviewOpen] = useState(false);
  const [previewImage, setPreviewImage] = useState('');
  const [previewTitle, setPreviewTitle] = useState('');
  const [fileList, setFileList] = useState([]);

  const [validateStatus, setValidateStatus] = useState('null');
  const [help, setHelp] = useState('');
  const [workingTime, setWorkingTime] = useState('');

  //address cascader
  const [selectedAddress, setSelectedAddress] = useState([]);
  const addressCascaderProps = {
    style: { width: '100%' },
    placeholder: 'Chọn địa chỉ',
  };
  const onCascaderChange = useCallback((value, selectedOptions) => {
    setSelectedAddress(selectedOptions ?? []);
    setValidateStatus('');
    setHelp('');
  }, []);
  //END cascader
  //goong map
  const [pointedLocation, setPointedLocation] = useState(null);
  const [placesFromLocation, setPlacesFromLocation] = useState([]);
  const [sessionToken, setSessionToken] = useState(null);
  const [locationSearchValue, setLocationSearchValue] = useState(null)
  const locationSearchValueDebound = useDebounce(locationSearchValue, 600)
  const [selectedLocationDetail, setSelectedLocationDetail] = useState(null);


  const goongMapService = useGoongMapService();
  function onDragEnd() {
    var lngLat = marker.getLngLat();
    setPointedLocation({ lng: lngLat.lng, lat: lngLat.lat })
  }

  const onLocationSearch = (value) => {
    const addressCombine = [value, ...selectedAddress.reverse().map((a) => a.name)].join(', ')
    if (!addressCombine) return;
    goongMapService.placeAutoComplete(addressCombine, sessionToken)
      .then(res => setPlacesFromLocation(res.data))
  }
  const onAddressSelected = (placeId) => {
    goongMapService
      .getPlaceDetail(placeId, sessionToken)
      .then(res => {
        map.jumpTo({ zoom: map.getZoom(), center: res.data.geometry.position })
        marker.setLngLat(res.data.geometry.position)
        setSelectedLocationDetail(res.data);
        setSessionToken(uuidv4())
      })
  }
  useEffect(() => {
    setSessionToken(uuidv4())
    initMap(defaulLocation.geometry.position)
    marker.on("dragend", onDragEnd)
    return () => {
      marker.remove()
      map.remove()

    }
  }, [])
  useMemo(() => {
    if (!pointedLocation?.lng || !pointedLocation?.lat) return;
    goongMapService
      .getPlaceFromLocation(pointedLocation)
      .then(res => setPlacesFromLocation(res.data));
  }, [JSON.stringify(pointedLocation)])
  useMemo(() => {
    onLocationSearch(locationSearchValueDebound)
  }, [locationSearchValueDebound])


  //end goong map

  const uploadButton = (
    <div>
      <PlusOutlined />
      <div
        style={{
          marginTop: 8,
        }}
      >
        Upload
      </div>
    </div>
  );


  const handleCancel = () => setPreviewOpen(false);
  const handlePreview = async (file) => {
    if (!file.url && !file.preview) {
      file.preview = await getBase64(file.originFileObj);
    }
    setPreviewImage(file.url || file.preview);
    setPreviewOpen(true);
    setPreviewTitle(file.name || file.url.substring(file.url.lastIndexOf('/') + 1));
  };
  const handleChange = ({ fileList: newFileList }) => setFileList(newFileList);
  const handelChangeTime = (_, timeString) => {
    setWorkingTime(timeString);
  };
  const onFinish = (values) => {
    if (!selectedAddress) {
      setValidateStatus('error');
      setHelp('Vui lòng chọn địa chỉ của bãi đỗ xe');
    } else {
      values = { ...values, parkingZoneImages: fileList, communeId: selectedAddress?.id };

      const formData = new FormData();
      formData.append('ownerId', account.UserId);
      formData.append('name', values.name);
      formData.append('pricePerHour', values.pricePerHour);
      formData.append('priceOverTimePerHour', values.priceOverTimePerHour);
      formData.append('slots', values.slots);
      formData.append('detailAddress', selectedLocationDetail.formattedAddress);
      values.parkingZoneImages.forEach((item) => {
        formData.append('parkingZoneImages', item.originFileObj);
      });
      formData.append('workFrom', workingTime[0]);
      formData.append('workTo', workingTime[1]);
      formData.append('location.lat', selectedLocationDetail.geometry.position.lat);
      formData.append('location.lng', selectedLocationDetail.geometry.position.lng);

      parkingZoneService.register(formData);
    }
  };

  return (
    <div className="w-full">
      <Row gutter={8}>
        <Col span={12}>

          <Form {...layout} name="nest-messages" onFinish={onFinish} validateMessages={validateMessages}>
            <Form.Item
              name="name"
              label="Tên"
              rules={[
                {
                  required: true,
                },
              ]}
            >
              <Input placeholder="Đặt tên cho parking zone" />
            </Form.Item>
            <Form.Item
              name="pricePerHour"
              label="Giá tiền mỗi giờ"
              rules={[
                {
                  required: true,
                },
              ]}
            >
              <InputNumber style={{ width: "100%" }} prefix="VND" />
            </Form.Item>
            <Form.Item
              name="priceOverTimePerHour"
              label="Giá tiền quá giờ"
              rules={[
                {
                  required: true,
                },
              ]}
            >
              <InputNumber style={{ width: "100%" }} prefix="VND" />
            </Form.Item>
            <Form.Item name="workingTime" label="Thời gian làm việc" required>
              <TimePicker.RangePicker
                style={{ width: "100%" }}
                onChange={handelChangeTime}
                placeholder={["Giờ mở cửa", "Giờ đóng cửa"]}
              />
            </Form.Item>
            <Form.Item
              name="slots"
              label="Slots"
              rules={[
                {
                  required: true,
                },
              ]}
            >
              <InputNumber style={{ width: "100%" }} placeholder="Số slots của parking zone" />
            </Form.Item>
            <Form.Item name={'detailAddress'} label="Địa chỉ" validateStatus={validateStatus} help={help}>
              <Space.Compact direction='vertical' style={{ width: "100%" }}>
                <AddressCascader cascaderProps={addressCascaderProps} onCascaderChangeCallback={onCascaderChange} />
                <Select
                  showSearch
                  allowClear
                  filterOption={() => true}
                  options={placesFromLocation.map((p) => {
                    return {
                      value: p.placeId,
                      label: p.description ?? p.formattedAddress
                    }
                  })}
                  placeholder="Địa chỉ cụ thể"
                  onSearch={setLocationSearchValue}
                  onSelect={onAddressSelected}
                  onClear={() => { setSessionToken(uuidv4()) }}
                />
              </Space.Compact>
            </Form.Item>

            <Form.Item
              name="parkingZoneImages"
              label="Ảnh bãi đỗ xe"
              rules={[
                {
                  required: true,
                },
              ]}
            >
              <div>
                <Upload
                  accept="image/*"
                  listType="picture-card"
                  fileList={fileList}
                  onPreview={handlePreview}
                  onChange={handleChange}
                  beforeUpload={() => false}
                  maxCount={8}
                >
                  {uploadButton}
                </Upload>
              </div>
            </Form.Item>
            <Form.Item
              className={('flex justify-center m-0')}
            >
              <Button className="bg-[#1677ff]" type="primary" htmlType="submit">
                Submit
              </Button>
            </Form.Item>
          </Form>
          <Modal open={previewOpen} title={previewTitle} footer={null} onCancel={handleCancel}>
            <img
              alt="example"
              style={{
                width: '100%',
              }}
              src={previewImage}
            />
          </Modal>
        </Col>
        <Col span={12} style={{ position: 'relative' }}>

          <div id='register-pz-map' style={{ position: "absolute", left: 0, top: 0, width: '100%', height: '100%' }}>
            <h4 style={{ right: 0, textAlign: "end" }}><i>***Chọn một điểm để xác định vị trí chính xác của nhà xe</i></h4>
          </div>
        </Col>
      </Row>

    </div>
  );
};
export default RegisterParkingZone;
