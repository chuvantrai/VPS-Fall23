import { Button, Form, Input, InputNumber, Modal, notification, Select } from 'antd';
import React, { useEffect, useState } from 'react';
import optionsCreateAddressType from '@/helpers/optionsCreateAddressType.js';
import useAddressServices from '@/services/addressServices.js';
import addressTypeEnum from '@/helpers/addressTypeEnum.js';

const CreateAddress = ({ callBackCreateAddress }) => {
  let options = optionsCreateAddressType;
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [form] = Form.useForm();
  const [typeCityOptions, setTypeCityOptions] = useState([]);
  const [typeDistrictOptions, setTypeDistrictOptions] = useState([]);
  const [districtFilter, setDistrictFilter] = useState(undefined);
  const [cityFilter, setCityFilter] = useState(undefined);
  const [addressType, setAddressType] = useState(options[0].value);

  const service = useAddressServices();

  const showModal = () => {
    setIsModalOpen(true);
  };
  const handleCancel = () => {
    setIsModalOpen(false);
  };

  const onChangeAddressType = (val) => {
    setAddressType(val);
    setDistrictFilter(undefined);
    setCityFilter(undefined);
    form.setFieldValue('city', undefined);
    form.setFieldValue('district', undefined);
  };
  const onFinish = (val) => {
    switch (addressType) {
      case addressTypeEnum.DISTRICT:
        if (cityFilter === undefined) {
          notification.error({
            message: 'Lỗi',
            description: 'Chưa chọn tỉnh/Thành phố',
            placement: 'topRight',
          });
          return;
        }
        break;
      case addressTypeEnum.COMMUNE:
        if (cityFilter === undefined) {
          notification.error({
            message: 'Lỗi',
            description: 'Chưa chọn tỉnh/Thành phố',
            placement: 'topRight',
          });
          return;
        }
        if (districtFilter === undefined) {
          notification.error({
            message: 'Lỗi',
            description: 'Chưa chọn quận/Huyện',
            placement: 'topRight',
          });
          return;
        }
        break;
    }
    service.createAddress(val).then((data) => {
      notification.success({
        message: 'Tạo địa chỉ thành công',
        placement: 'topRight',
      });
      setIsModalOpen(false);
      callBackCreateAddress();
    });
  };

  const OnLoadDistrictFilter = (val) => {
    if (val === undefined) {
      setTypeDistrictOptions([]);
    } else {
      service.getDistricts(val).then((res) => {
        setTypeDistrictOptions(res.data.map((val) => {
            return {
              value: val.id,
              label: val.name,
            };
          }),
        );
        setDistrictFilter(undefined);
      });
    }
  };

  const OnChangeCityFilter = (val) => {
    setCityFilter(val);
    setDistrictFilter(undefined);
    form.setFieldValue('district', undefined);
    OnLoadDistrictFilter(val);
  };
  const OnChangeDistrictFilter = (val) => {
    setDistrictFilter(val);
  };

  const loadTypeCityOptions = () => {
    service.getCities().then((res) => {
      setTypeCityOptions(res.data.map((val) => {
          return {
            value: val.id,
            label: val.name,
          };
        }),
      );
    });
  };

  const afterOpenChange1 = (val) => {
    if (val) {
      loadTypeCityOptions();
    }
  };

  const OnFilterOption = (input, option) => (option?.label ?? '').toLowerCase().includes(input.toLowerCase());

  useEffect(() => {
    form.setFieldValue('type', addressType);
  }, [districtFilter, cityFilter]);
  return (<>
        <span onClick={showModal}>
            <Button className={'bg-[#1890FF]'} type='primary'>Thêm địa chỉ </Button>
        </span>
    <Modal
      afterOpenChange={(e) => afterOpenChange1(e)}
      confirmLoading={true}
      title='Tạo địa chỉ mới'
      open={isModalOpen}
      okButtonProps={{
        style: {
          backgroundColor: '#1677ff', display: 'none',
        },
      }}
      cancelButtonProps={{
        style: {
          display: 'none',
        },
      }}
      onCancel={handleCancel}>
      <Form
        labelCol={{ span: 6 }}
        wrapperCol={{ span: 17 }}
        form={form}
        onFinish={onFinish}
      >
        <Form.Item
          label='Loại đại chỉ'
          name='type'
          defaultValue={options[0].value}
        >
          <Select
            className={('min-w-[230px]')}
            defaultValue={options[0]}
            style={{ width: 120 }}
            options={options}
            onChange={onChangeAddressType}
          />
        </Form.Item>
        <div className={('flex w-full ')}>
          {
            addressType === addressTypeEnum.COMMUNE || addressType === addressTypeEnum.DISTRICT ?
              <Form.Item
                name='city'
                className={('w-[50%] flex justify-center')}
              >
                <Select
                  className={('w-[20rem!important]')}
                  showSearch
                  placeholder='Tỉnh/Thành phố'
                  optionFilterProp='children'
                  allowClear
                  onChange={OnChangeCityFilter}
                  filterOption={OnFilterOption}
                  options={typeCityOptions}
                />
              </Form.Item>
              : <></>
          }
          {
            addressType === addressTypeEnum.COMMUNE ?
              <Form.Item
                name='district'
                className={('w-[50%] flex justify-center')}
              >
                <Select
                  className={('w-[20rem!important]')}
                  showSearch
                  placeholder='Quận/Huyện'
                  optionFilterProp='children'
                  allowClear
                  onChange={OnChangeDistrictFilter}
                  filterOption={OnFilterOption}
                  options={typeDistrictOptions}
                  value={districtFilter}
                />
              </Form.Item>
              : <></>
          }
        </div>
        <Form.Item
          label='Mã địa chỉ'
          name='code'
          rules={[{
            required: true, message: 'Không để trống nội dung!',
          }]}
        >
          <InputNumber placeholder='Mã địa chỉ' className='w-[100%]' />
        </Form.Item>
        <Form.Item
          label='Tên địa chỉ'
          name='name'
          rules={[{
            required: true, message: 'Không để trống nội dung!',
          }, {
            max: 100, message: 'Nội dung tối đa 100 ký tự!',
          }]}
        >
          <Input placeholder='Tên địa chỉ'></Input>
        </Form.Item>
        <Form.Item className={('flex justify-center m-0')}>
          <Button className={('bg-[#1890FF] w-[200%]')} type='primary' htmlType={'submit'}>
            Lưu
          </Button>
        </Form.Item>
      </Form>
    </Modal>
  </>);
};

export default CreateAddress;