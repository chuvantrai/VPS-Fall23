import classNames from 'classnames/bind';
import styles from './AccountProfile.module.scss';
import { Button, Form, Input } from 'antd';
import TextArea from 'antd/es/input/TextArea.js';
import { useRef, useState, useEffect, useCallback } from 'react';
import { UploadOutlined } from '@ant-design/icons';
import AddressCascader from '@/components/cascader/AddressCascader.jsx';
import AccountServices from '@/services/accountServices.js';
import getAccountJwtModel from '../../helpers/getAccountJwtModel.js';


const cx = classNames.bind(styles);

function AccountProfile() {
  const [form] = Form.useForm();
  const [validateStatus, setValidateStatus] = useState('null');
  const [help, setHelp] = useState('');
  const [selectedAddress, setSelectedAddress] = useState(null);
  const addressCascaderProps = {
    style: { width: '100%' },
    placeholder: 'Chọn địa chỉ',
  };
  const [stringArray,setstringArray] = useState([]);
  const onCascaderChange = useCallback((value, selectedOptions) => {
    setSelectedAddress(selectedOptions ? selectedOptions[selectedOptions.length - 1] : null);
    setValidateStatus('');
    setHelp('');
  }, []);
  const accountProfile = AccountServices();

  const onFinish = (values) => {
    accountProfile.updateAccountProfile(values,selectedAddress?.id, fileImage);
  };
  useEffect(() => {
    const handleLoad = () => {
      accountProfile.getAccountProfile(form,callbackAddress);
    };
    handleLoad();
    window.addEventListener('load', handleLoad);
    return () => {
      window.removeEventListener('load', handleLoad);
    };
  }, []);

  const callbackAddress = (value,idCommune) => {
      setstringArray(value);
      if(idCommune!==''){
        setSelectedAddress({...selectedAddress,id: idCommune});
      }
  }

  // avartar update
  const [selectedImage, setSelectedImage] = useState(null);
  const [fileImage, setFileImage] = useState(null);
  const inputFileRef = useRef();
  const handleButtonUpload = () => {
    inputFileRef.current.click();
  };
  const handleImageUpload = (event) => {
    const file = event.target.files[0];
    if (file !== undefined) {
      setFileImage(file);
      const reader = new FileReader();
      reader.onload = (upload) => {
        setSelectedImage(upload.target.result);
      };
      reader.readAsDataURL(file);
    }
  };
  // avartar update

  return (
    <div className={cx('w-full pl-[20px] pr-[20px] min-h-[calc(100vh-250px)] mt-[20px] page-account-profile')}>
      <Form
        className={cx('grid-rows-2 gap-4')}
        layout={'vertical'}
        form={form}
        name='userProfile'
        onFinish={onFinish}
        scrollToFirstError
      >
        <div className={cx('grid grid-cols-3 row-span-1 gap-4')}>
          <div className={cx('col-span-2')}>
            {/* START Row 1 (first Name, Last Name)*/}
            <div className={cx('t grid grid-cols-2 gap-4')}>
              <div className={cx('grid-rows-2 gap-4')}>
                <div className={cx('row-span-1 ')}>
                  <Form.Item
                    label='Tên'
                    name='firstName'
                    rules={[
                      {
                        required: true,
                        message: 'Hãy điền tên của bạn!',
                      },
                      {
                        max: 20,
                        message: 'Tên tối đa chỉ được 20 ký tự!',
                      },
                    ]}
                  >
                    <Input placeholder='Tên' />
                  </Form.Item>
                </div>
              </div>
              <div className={cx('grid-rows-2 gap-4')}>
                <div className={cx('row-span-1 ')}>
                  <Form.Item
                    label='Họ'
                    name='lastName'
                    rules={[
                      {
                        required: true,
                        message: 'Hãy điền Họ của bạn!',
                      },
                      {
                        max: 20,
                        message: 'Họ tối đa chỉ được 20 ký tự!',
                      },
                    ]}
                  >
                    <Input placeholder='Họ' />
                  </Form.Item>
                </div>
              </div>
            </div>
            {/* END Row 1 (first Name, Last Name)*/}

            {/* START Row 2 (phone, email)*/}
            <div className={cx('t grid grid-cols-2 gap-4')}>
              <div className={cx('grid-rows-2 gap-4')}>
                <div className={cx('row-span-1 ')}>
                  <Form.Item
                    label='Số ĐT'
                    name='phone'
                    rules={[
                      {
                        required: true,
                        message: 'Hãy điền số điện thoại của bạn!',
                      },
                      {
                        max: 10,
                        message: 'Số điện thoại tối đa chỉ được 10 ký tự',
                      },
                      {
                        pattern: /(0[3|5|7|8|9])+([0-9]{8})\b/g,
                        message: 'Sai định dạng',
                      },
                    ]}
                  >
                    <Input placeholder='Số điện thoại' />
                  </Form.Item>
                </div>
              </div>
              <div className={cx('grid-rows-2 gap-4')}>
                <div className={cx('row-span-1 ')}>
                  <Form.Item
                    name='email'
                    label='Email'
                  >
                    <Input value='0362351671@gmail.com' disabled />
                  </Form.Item>
                </div>
              </div>
            </div>
            {/* END Row 2 (phone, email)*/}

            {/* START Row 3 (City, District, Commune)*/}
            <div className={cx('t grid grid-cols-2 gap-4')}>
              <div className={cx('grid-rows-2 gap-4')}>
                <div className={cx('row-span-1 ')}>
                  <Form.Item
                    name='commune'
                    label='Địa chỉ'
                    className='pb-[24px] m-0'
                    validateStatus={validateStatus}
                    help={help}
                  >
                    <div className='w-full'>
                      <AddressCascader
                        cascaderProps={addressCascaderProps}
                        onCascaderChangeCallback={onCascaderChange}
                        defaultAddress={stringArray}
                        />
                    </div>
                  </Form.Item>
                </div>
              </div>
              <div className={cx('grid-rows-2 gap-4')}>
                <div className={cx('row-span-1 ')}>
                  <Form.Item
                    name='role'
                    className={cx('mb-[15px]')}
                    label='Vai Trò'
                  >
                    <Input value='ADMIN' disabled />
                  </Form.Item>
                </div>
              </div>
            </div>
            {/* END Row 3 (City, District, Commune)*/}

            {/* START Row 4 (Address, DOB)*/}
            <div className={cx('t grid grid-cols-2 gap-4')}>
              <div className={cx('grid-rows-2 gap-4')}>
                <div className={cx('row-span-1 ')}>
                  <Form.Item
                    label='Địa chỉ chi tiết'
                    name='address'
                    rules={[
                      {
                        max: 100,
                        message: 'Địa chỉ tối đa 100 ký tự!',
                      },
                    ]}
                  >
                    <TextArea className={cx('max-h-[97px]')} rows={4} placeholder='Địa chỉ tối đa 100 ký tự!' />
                  </Form.Item>
                </div>
              </div>
              {form.getFieldValue('roleId')===2?
                <div className={cx('grid-rows-2 gap-4')}>
                  <div className={cx('row-span-1 ')}>
                    <Form.Item
                      name='dob'
                      className={cx('mb-[15px]')}
                      label='Ngày sinh'
                    >
                      <Input value='dateDob' disabled />
                    </Form.Item>
                  </div>
                </div>:<span></span>
              }

            </div>
            {/* START Row 4 (Address, DOB)*/}
          </div>
          <div className={cx('col-span-1 grid-rows-2')}>
            {/* START Row 1 (IMG)*/}
            <div className={cx('')}>
              {selectedImage ? (
                  <div className={cx('flex justify-center ')}>
                    <img
                      src={selectedImage}
                      alt='Selected Image'
                      className={cx('object-cover rounded-[15px] w-3/5 aspect-[1] overflow-hidden')}
                    />
                  </div>
                ) :
                <div className={cx('flex justify-center ')}>
                  <img
                    src={form.getFieldValue('avatar')??'../src/assets/images/AvatarDefault.png'}
                    alt='Selected Image'
                    className={cx('object-cover rounded-[15px] w-3/5 aspect-[1] overflow-hidden')}
                  />
                </div>
              }
              <div className={cx('flex justify-center w-full mt-[10px]')}>
                <input
                  type='file'
                  className={cx('hidden')}
                  style={{ display: 'none' }}
                  accept='.jpeg, .jpg, .png'
                  onChange={handleImageUpload}
                  ref={inputFileRef} />
                <Button
                  icon={<UploadOutlined />}
                  onClick={handleButtonUpload}>Ảnh</Button>
              </div>
            </div>
            {/* END Row 1 (IMG)*/}
          </div>
          <div className={cx('col-span-3 h-[100px]')}>
            <Form.Item className={cx('flex justify-center')}>
              <Button className={cx('bg-[#1890FF] w-[200%]')} type='primary' htmlType='submit'>
                Lưu thay đổi
              </Button>
            </Form.Item>
          </div>
        </div>
        <div className={cx('row-span-1')}>
        </div>
      </Form>
    </div>

  );
}

export default AccountProfile;