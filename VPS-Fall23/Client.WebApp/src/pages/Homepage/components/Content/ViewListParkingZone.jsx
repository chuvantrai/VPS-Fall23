/* eslint-disable no-unused-vars */
/* eslint-disable react-hooks/exhaustive-deps */
import React, { useEffect, useState, useCallback, Fragment } from 'react';
import dayjs from 'dayjs';
import {
  Table,
  Modal,
  Button,
  Descriptions,
  Tag,
  Badge,
  Carousel,
  Image,
  Switch,
  Space,
  Form,
  Input,
  InputNumber,
  Upload,
  TimePicker,
  Pagination,
  AutoComplete,
  Divider,
  Popconfirm,
  notification,
} from 'antd';
import { FormOutlined, UploadOutlined, QuestionCircleOutlined } from '@ant-design/icons';

import useParkingZoneService from '@/services/parkingZoneService';
import { getAccountJwtModel } from '@/helpers';
import AddressCascader from '@/components/cascader/AddressCascader';
import { useDebounce } from 'use-debounce';
import CloseModalForm from './components/CloseModalForm';
import { Link, Outlet } from 'react-router-dom';

const onChange = (pagination, filters, sorter, extra) => {
  console.log('params', pagination, filters, sorter, extra);
};

function ViewListParkingZone() {
  const [form] = Form.useForm();

  const parkingZoneService = useParkingZoneService();
  const account = getAccountJwtModel();

  const [isModalEditOpen, setIsModalEditOpen] = useState(false);
  const [isModalViewOpen, setIsModalViewOpen] = useState(false);
  const [checkedState, setCheckedState] = useState(false);
  const [parkingZoneDetail, setParkingZoneDetail] = useState({});
  const [fileList, setFileList] = useState([]);
  const [selectedAddress, setSelectedAddress] = useState(null);
  const [validateStatus, setValidateStatus] = useState('null');
  const [help, setHelp] = useState('');
  const [address, setAddress] = useState([]);
  const [workingTime, setWorkingTime] = useState('');
  const [openCloseModal, setOpenCloseModal] = useState(false);
  const [parkingZoneId, setParkingZoneId] = useState('');

  const handleGetParkingZoneDetail = async (parkingZoneId, isApprove) => {
    await parkingZoneService.getParkingZoneDetail(parkingZoneId).then((res) => {
      const data = res.data;
      setParkingZoneDetail(data);
      form.setFieldsValue({ ...data, workingTime: [dayjs(data.workFrom, 'HH:mm:ss'), dayjs(data.workTo, 'HH:mm:ss')] });
      setAddress([data.city, data.district, data.commune]);
      setCheckedState(data.isFull);

      let files = [];
      for (let i = 0; i < data.parkingZoneImages.length; i++) {
        files.push({
          uid: i,
          name: 'img-' + i,
          status: 'done',
          url: data.parkingZoneImages[i],
          thumbUrl: data.parkingZoneImages[i],
        });
      }
      setFileList(files);
    });

    if (account.RoleId === '1') {
      setIsModalViewOpen(true);
    } else if (account.RoleId === '2') {
      if (isApprove === true) {
        setIsModalViewOpen(true);
      } else {
        setIsModalEditOpen(true);
      }
    }
  };

  const handleCancel = () => {
    form.resetFields();
    setIsModalEditOpen(false);
    setIsModalViewOpen(false);
  };
  const handleViewOk = () => {
    setIsModalViewOpen(false);
  };

  const [data, setData] = useState([{ key: '', id: '', name: '', owner: '', status: 'null', created: Date }]);

  const [inputValue, setInputValue] = useState('');

  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 10;
  const [totalItems, setTotalItems] = useState();

  // eslint-disable-next-line no-unused-vars
  const [debouncedValue] = useDebounce(inputValue, 1000);

  const handleSearch = (e) => {
    searchDataByName(e);
    setInputValue(e);
  };

  const columns = [
    {
      title: 'Tên bãi đỗ xe',
      dataIndex: 'name',
      key: 'name',
      render: (val) => (
        <Fragment>
          <Link to={`/${val}`}>{val}</Link>
        </Fragment>
      ),
    },
    {
      title: 'Chủ bãi đỗ xe',
      dataIndex: 'owner',
      key: 'owner',
    },
    {
      title: 'Ngày tạo',
      dataIndex: 'created',
      key: 'created',
    },
    {
      title: 'Trạng thái duyệt',
      dataIndex: 'status',
      key: 'status',
      render: (val) => (
        <Fragment>
          {val === null && (
            <Tag color="processing">
              <a>Đang duyệt</a>
            </Tag>
          )}
          {val === false && (
            <Tag color="red">
              <a>Đã từ chối</a>
            </Tag>
          )}
          {val === true && (
            <Tag color="success">
              <a>Đã duyệt</a>
            </Tag>
          )}
        </Fragment>
      ),
    },
    {
      title: '',
      key: 'action',
      render: (_, record) => (
        <Space split={<Divider type="vertical" />}>
          <a
            onClick={(e) => {
              e.preventDefault();
              handleGetParkingZoneDetail(record.key, record.status);
            }}
          >
            <FormOutlined />
          </a>

          {record.status === true ?
            <Popconfirm
              title="Đóng cửa bãi đỗ xe"
              description="Bạn có chắc muốn đóng cửa bãi đỗ xe này?"
              onConfirm={() => {
                handleOpenCloseModal(record.key);
              }}
              onCancel={() => {
                console.log('cancel');
              }}
              icon={
                <QuestionCircleOutlined
                  style={{
                    color: 'red',
                  }}
                />
              }
            >
              <Button danger>Đóng cửa</Button>
            </Popconfirm> :
            <Popconfirm
              title="Đóng cửa bãi đỗ xe"
              description="Bạn có chắc muốn đóng cửa bãi đỗ xe này?"
              onConfirm={() => {
                handleOpenCloseModal(record.key);
              }}
              onCancel={() => {
                console.log('cancel');
              }}
              icon={
                <QuestionCircleOutlined
                  style={{
                    color: 'red',
                  }}
                />
              }
            >
              <Button danger disabled>Đóng cửa</Button>
            </Popconfirm>}
        </Space>
      ),
    },
  ];

  useEffect(() => {
    getData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const getData = async (currentPage) => {
    if (inputValue === '' || inputValue === undefined || inputValue === null) {
      await parkingZoneService
        .getAllParkingZone({ pageNumber: currentPage, pageSize: pageSize })
        .then((res) => {
          const obj = res.data.data.map((val) => ({
            key: val.id,
            name: val.name,
            owner: val.owner,
            status: val.status,
            created: val.created,
          }));
          setData(obj);
          setTotalItems(res?.data.totalCount);
        })
        .catch((error) => {
          console.error('Error fetching data:', error);
        });
    }
  };

  const searchDataByName = async (e) => {
    if (e !== '') {
      await parkingZoneService
        .getParkingZoneByName({ pageNumber: currentPage, pageSize: pageSize, name: e })
        .then((res) => {
          const obj = res.data.data.map((val) => ({
            key: val.id,
            name: val.name,
            owner: val.owner,
            status: val.status,
            created: val.created,
          }));
          setData(obj);
          console.log(obj);
          setTotalItems(res?.data.totalCount);
        })
        .catch((error) => {
          console.error('Error fetching data:', error);
        });
    }
  };

  const handleOpenCloseModal = (parkingZoneId) => {
    setOpenCloseModal(true);
    setParkingZoneId(parkingZoneId);
  };

  const handleChangePage = (page) => {
    setCurrentPage(page);
    getData(page);
  };

  let currentDate = new Date();
  let currentTime = currentDate.getHours() + ':' + currentDate.getMinutes() + ':' + currentDate.getSeconds();
  let comparingCurrentTime = Date.parse('01/01/2001 ' + currentTime);
  let comparingWorkFrom = Date.parse('01/01/2001 ' + parkingZoneDetail.workFrom);
  let comparingWorkTo = Date.parse('01/01/2001 ' + parkingZoneDetail.workTo);

  const isActive = () => {
    if (parkingZoneDetail.parkingZoneAbsents?.length > 0) {
      return <Badge status="error" text="Ngừng hoạt động" />;
    } else {
      if (comparingWorkFrom < comparingCurrentTime && comparingCurrentTime < comparingWorkTo) {
        return <Badge status="success" text="Đang hoạt động" />;
      } else {
        return <Badge status="error" text="Ngừng hoạt động" />;
      }
    }
  };

  const formatter = new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND',
  });

  const descItems = [
    {
      key: '1',
      label: 'Tên',
      children: parkingZoneDetail.name,
    },
    {
      key: '2',
      label: 'Ngày tạo',
      children: parkingZoneDetail.createdAt,
    },
    {
      key: '3',
      label: 'Giá tiền (mỗi tiếng)',
      children: formatter.format(parkingZoneDetail.pricePerHour),
    },
    {
      key: '4',
      label: 'Giá tiền quá giờ (mỗi tiếng)',
      children: formatter.format(parkingZoneDetail.priceOverTimePerHour),
    },
    {
      key: '5',
      label: 'Số vị trí',
      children: <Tag color="processing">{parkingZoneDetail.slots}</Tag>,
    },
    {
      key: '6',
      label: 'Trạng thái',
      children: isActive(),
    },
    {
      key: '7',
      label: 'Thời gian làm việc',
      children: parkingZoneDetail.workFrom + ' - ' + parkingZoneDetail.workTo,
      span: 2,
    },
    {
      key: '8',
      label: 'Địa chỉ chi tiết',
      children: parkingZoneDetail.detailAddress,
      span: 2,
    },
  ];

  const addressCascaderProps = {
    style: { width: '100%' },
    placeholder: 'Chọn địa chỉ',
  };

  const onCascaderChange = useCallback((value, selectedOptions) => {
    setSelectedAddress(selectedOptions ? selectedOptions[selectedOptions.length - 1] : null);
    setValidateStatus('');
    setHelp('');
  }, []);

  const handleChange = ({ fileList: newFileList }) => setFileList(newFileList);

  const handleChangeSwitch = (checked) => {
    const params = {
      parkingZoneId: parkingZoneDetail.id,
      isFull: checked,
    };
    parkingZoneService.changeParkingZoneFullStatus(params);
    setCheckedState(checked);
  };

  const handelChangeTime = (_, timeString) => {
    setWorkingTime(timeString);
  };

  const handleSubmitForm = (values) => {
    if (!selectedAddress) {
      setValidateStatus('error');
      setHelp('Vui lòng chọn địa chỉ của bãi đỗ xe');
    } else {
      values = { ...values, parkingZoneImages: fileList, communeId: selectedAddress?.id };

      const formData = new FormData();
      formData.append('parkingZoneId', values.id);
      formData.append('parkingZoneName', values.name);
      formData.append('pricePerHour', values.pricePerHour);
      formData.append('priceOverTimePerHour', values.priceOverTimePerHour);
      formData.append('slots', values.slots);
      formData.append('workFrom', workingTime[0]);
      formData.append('workTo', workingTime[1]);
      formData.append('communeId', values.communeId);
      formData.append('detailAddress', values.detailAddress);
      values.parkingZoneImages.forEach((item) => {
        formData.append('parkingZoneImages', item.originFileObj);
      });

      // parkingZoneService.updateParkingZone(formData);
      // console.log(values);
      console.log('parkingZoneId: ' + formData.get('parkingZoneId'));
      console.log('parkingZoneName: ' + formData.get('parkingZoneName'));
      console.log('pricePerHour: ' + formData.get('pricePerHour'));
      console.log('priceOverTimePerHour: ' + formData.get('priceOverTimePerHour'));
      console.log('slots: ' + formData.get('slots'));
      console.log('workFrom: ' + formData.get('workFrom'));
      console.log('workTo' + formData.get('workTo'));
      console.log('communeId: ' + formData.get('communeId'));
      console.log('detailAddress: ' + formData.get('detailAddress'));
      console.log('parkingZoneImages: ' + formData.get('parkingZoneImages'));
    }
  };

  const handleSubmitCloseParkingZoneForm = (values) => {
    let input = {};
    if (typeof values.closeTime === 'object') {
      input = {
        parkingZoneId: values.parkingZoneId,
        reason: values.reason,
        closeFrom: values.closeTime[0],
        closeTo: values.closeTime[1],
      };
    } else {
      input = {
        parkingZoneId: values.parkingZoneId,
        reason: values.reason,
        closeFrom: values.closeTime,
      };
    }
    parkingZoneService.closeParkingZone(input);
    setOpenCloseModal(false);
  };

  return (
    <Fragment>
      <Outlet></Outlet>
      <div className="w-full px-4">
        <AutoComplete style={{ width: 200 }} onSearch={handleSearch} placeholder="Tìm kiếm" className="mt-4 mb-4" />
        {data != undefined && (
          <Fragment>
            <Table columns={columns} dataSource={data} onChange={onChange} pagination={false} />
            <div className="py-[16px] flex flex-row-reverse pr-[24px]">
              <Pagination
                current={currentPage}
                onChange={handleChangePage}
                total={totalItems}
                showSizeChanger={false}
              />
            </div>
          </Fragment>
        )}
      </div>
      <Modal
        centered
        title="Thông tin bãi đỗ xe"
        open={isModalEditOpen}
        onCancel={handleCancel}
        footer={(_, { CancelBtn }) => (
          <Space>
            <Switch
              checkedChildren="Full"
              unCheckedChildren="Available"
              onChange={handleChangeSwitch}
              checked={checkedState}
            />
            <CancelBtn />
            <Button
              className="bg-[#1677ff] text-white"
              onClick={() => {
                form.validateFields().then((values) => {
                  handleSubmitForm(values);
                });
              }}
            >
              Lưu
            </Button>
          </Space>
        )}
        width={600}
      >
        <Form form={form} name="editForm" style={{ maxWidth: '600px' }} layout="vertical">
          <Form.Item className="hidden" name="id">
            <Input type="hidden" />
          </Form.Item>
          <Form.Item
            name="name"
            label="Tên"
            rules={[
              {
                required: true,
                message: 'Không thể để trống',
              },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="pricePerHour"
            label="Giá tiền (mỗi giờ)"
            rules={[
              {
                required: true,
                message: 'Không thể để trống',
              },
            ]}
          >
            <InputNumber className="w-[100%]" prefix="VND" />
          </Form.Item>
          <Form.Item
            name="priceOverTimePerHour"
            label="Giá tiền quá giờ (mỗi giờ)"
            rules={[
              {
                required: true,
                message: 'Không thể để trống',
              },
            ]}
          >
            <InputNumber className="w-[100%]" prefix="VND" />
          </Form.Item>
          <Form.Item
            name="slots"
            label="Số chỗ"
            rules={[
              {
                required: true,
                message: 'Không thể để trống',
              },
            ]}
          >
            <InputNumber className="w-[100%]" />
          </Form.Item>
          <Form.Item
            name="workingTime"
            label="Thời gian làm việc"
            rules={[
              {
                required: true,
                message: 'Không thể để trống',
              },
            ]}
          >
            <TimePicker.RangePicker className="w-[100%]" onChange={handelChangeTime} />
          </Form.Item>
          <Form.Item
            name="communeId"
            label="Địa chỉ"
            validateStatus={validateStatus}
            help={help}
            rules={[
              {
                required: true,
                message: 'Không thể để trống',
              },
            ]}
          >
            <AddressCascader
              cascaderProps={addressCascaderProps}
              onCascaderChangeCallback={onCascaderChange}
              defaultAddress={address}
            />
          </Form.Item>
          <Form.Item
            name="detailAddress"
            label="Địa chỉ cụ thể"
            rules={[
              {
                required: true,
                message: 'Không thể để trống',
              },
            ]}
          >
            <Input.TextArea
              placeholder="Địa chỉ cụ thể"
              style={{
                height: '76px',
              }}
            />
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
            <Upload
              className="upload-list-inline"
              accept="image/*"
              listType="picture"
              fileList={fileList}
              onChange={handleChange}
              beforeUpload={() => false}
            >
              {fileList.length >= 8 ? null : <Button icon={<UploadOutlined />}>Upload</Button>}
            </Upload>
          </Form.Item>
        </Form>
      </Modal>

      <Modal
        title="Thông tin bãi đỗ xe"
        width={'40vw'}
        centered
        open={isModalViewOpen}
        onCancel={handleCancel}
        footer={() => (
          <Space>
            {account.RoleId === '2' && (
              <Switch
                checkedChildren="Full"
                unCheckedChildren="Available"
                onChange={handleChangeSwitch}
                checked={checkedState}
              />
            )}
            <Button className="bg-[#1677ff] text-white" onClick={handleViewOk}>
              OK
            </Button>
          </Space>
        )}
      >
        <Image.PreviewGroup>
          <Carousel className="mb-[10px]" autoplay dotPosition="top">
            {parkingZoneDetail.parkingZoneImages?.map((img, index) => (
              <Image key={index} src={img} />
            ))}
          </Carousel>
        </Image.PreviewGroup>
        <Descriptions bordered items={descItems} column={{ xs: 1, sm: 1, md: 1, lg: 1, xl: 2, xxl: 2 }} />
      </Modal>

      <CloseModalForm
        open={openCloseModal}
        parkingZoneId={parkingZoneId}
        onCancel={() => {
          setOpenCloseModal(false);
        }}
        onClose={handleSubmitCloseParkingZoneForm}
      />
    </Fragment>
  );
}

export default ViewListParkingZone;
