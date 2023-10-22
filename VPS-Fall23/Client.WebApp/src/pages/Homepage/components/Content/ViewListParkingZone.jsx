/* eslint-disable react-hooks/exhaustive-deps */
import React, { useCallback, useEffect, useState } from 'react';
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
} from 'antd';
import { FormOutlined } from '@ant-design/icons';

import useParkingZoneService from '@/services/parkingZoneService.js';
import { getAccountJwtModel } from '@/helpers';

const onChange = (pagination, filters, sorter, extra) => {
  console.log('params', pagination, filters, sorter, extra);
};

function ViewListParkingZone() {
  const columns = [
    {
      title: 'Tên',
      dataIndex: 'name',
    },
    {
      title: 'Chủ sở hữu',
      dataIndex: 'owner',
    },
    {
      title: 'Ngày tạo',
      dataIndex: 'created',
    },
    {
      title: 'Action',
      key: 'action',
      render: (_, record) => (
        <a
          onClick={(e) => {
            e.preventDefault();
            handleGetParkingZoneDetail(record.key, record.isApprove);
          }}
        >
          <FormOutlined />
        </a>
      ),
    },
  ];

  const [form] = Form.useForm();
  const formRef = React.createRef();

  const parkingZoneService = useParkingZoneService();
  const account = getAccountJwtModel();

  const [isModalEditOpen, setIsModalEditOpen] = useState(false);
  const [isModalViewOpen, setIsModalViewOpen] = useState(false);
  const [parkingZoneDetail, setParkingZoneDetail] = useState({});

  const handleGetParkingZoneDetail = useCallback((parkingZoneId, isApprove) => {
    parkingZoneService.getParkingZoneDetail(parkingZoneId).then((res) => {
      setParkingZoneDetail(res.data);
      console.log('data: ');
      console.log(res.data);
      console.log('detail 1: ');
      console.log(parkingZoneDetail);
    });

    if (account.RoleId === '1' || (account.RoleId === '2' && isApprove === true)) {
      setIsModalViewOpen(true);
    } else if (account.RoleId === '2') {
      setIsModalEditOpen(true);
    }
  });
  console.log('detail 2: ');
  console.log(parkingZoneDetail);

  const handleCancel = () => {
    form.resetFields();
    setIsModalEditOpen(false);
    setIsModalViewOpen(false);
  };
  const handleViewOk = () => {
    setIsModalViewOpen(false);
  };

  const [data, setData] = useState([{ id: '', name: '', owner: '', created: Date, isApprove: null }]);
  let dataShow = [{ key: '', name: '', owner: '', created: Date, isApprove: null }];

  useEffect(() => {
    getData();
  }, []);

  const getData = async () => {
    await parkingZoneService
      .getAllParkingZone()
      .then((res) => {
        setData(res.data);
      })
      .catch((error) => {
        console.error('Error fetching data:', error);
      });
  };

  let currentDate = new Date();
  let currentTime = currentDate.getHours() + ':' + currentDate.getMinutes() + ':' + currentDate.getSeconds();

  const isActive = () => {
    if (parkingZoneDetail.parkingZoneAbsents?.length > 0) {
      return <Badge status="error" text="Ngừng hoạt động" />;
    } else {
      if (parkingZoneDetail.workFrom > currentTime || parkingZoneDetail.workTo < currentTime) {
        return <Badge status="error" text="Ngừng hoạt động" />;
      } else {
        return <Badge status="success" text="Đang hoạt động" />;
      }
    }
  };

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
      children: parkingZoneDetail.pricePerHour,
    },
    {
      key: '4',
      label: 'Giá tiền quá giờ (mỗi tiếng)',
      children: parkingZoneDetail.priceOverTimePerHour,
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
      label: 'Địa chỉ chi tiết',
      children: parkingZoneDetail.detailAddress,
    },
  ];

  const formItemLayout = {
    labelCol: {
      xs: {
        span: 24,
      },
      sm: {
        span: 4,
      },
    },
    wrapperCol: {
      xs: {
        span: 24,
      },
      sm: {
        span: 20,
      },
    },
  };

  const handleSubmitForm = (values) => {
    console.log(values);
  };

  return (
    <>
      <div className="w-full px-4">
        {data !== undefined &&
          data.map((val) => {
            const item = {
              key: val.id,
              name: val.name,
              owner: val.owner,
              created: val.created,
              isApprove: val.isApprove,
            };
            dataShow.push(item);
          })}
        {dataShow.shift() && console.log('1')}
        {dataShow.length != 1 && dataShow[0].key !== '' && (
          <Table columns={columns} dataSource={dataShow} onChange={onChange} />
        )}
      </div>
      <Modal
        centered
        title="Thông tin bãi đỗ xe"
        open={isModalEditOpen}
        onCancel={handleCancel}
        footer={() => (
          <Space>
            <Switch checkedChildren="Full" unCheckedChildren="Available" />
            <Button className="bg-[#1677ff] text-white" onClick={() => formRef.current.submit()}>
              Lưu
            </Button>
          </Space>
        )}
        width={600}
      >
        <Form
          {...formItemLayout}
          form={form}
          ref={formRef}
          name="editForm"
          onFinish={handleSubmitForm}
          style={{ maxWidth: '600px' }}
          labelWrap
          initialValues={parkingZoneDetail}
        >
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
            <Switch checkedChildren="Full" unCheckedChildren="Available" />
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
    </>
  );
}

export default ViewListParkingZone;
