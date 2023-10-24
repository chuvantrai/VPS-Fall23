import classNames from 'classnames/bind';
import styles from './Test.module.scss';
import { Button, Form, Input, Modal, Select } from 'antd';
import { useState } from 'react';
import TextArea from 'antd/es/input/TextArea.js';
import optionsReportType from '@/helpers/optionsReportType.js';
import reportTypeEnum from '@/helpers/reportTypeEnum.js';
import GetAccountJwtModel from '@/helpers/getAccountJwtModel.js';
import userRoleEnum from '@/helpers/userRoleEnum.js';
import reportServices from '@/services/reportServices.js';


const cx = classNames.bind(styles);

const templateDriver = (
  <>
    <Form.Item
      label='Email của bạn'
      name='email'
      rules={[
        { required: true, message: 'Vui lòng nhập email của bạn' },
        {
          type: 'email',
          message: 'Email không hợp lệ',
        },
      ]}
    >
      <Input placeholder='Email'></Input>
    </Form.Item>
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
      <Input placeholder='Số điện thoại'></Input>
    </Form.Item>
  </>
);

const templatePaymentCode = (
  <>
    <Form.Item
      label='Mã giao dịch'
      name='paymentCode'
      rules={[
        { required: true, message: 'Vui lòng nhập email của bạn' },
      ]}
    >
      <Input placeholder='Mã giao dịch'></Input>
    </Form.Item>
  </>
);

function Test() {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const showModal = () => {
    setIsModalOpen(true);
  };
  const handleCancel = () => {
    setIsModalOpen(false);
  };
  let isDriver = false;

  const [form] = Form.useForm();
  let options = optionsReportType;
  const account = GetAccountJwtModel();
  if (account?.RoleId === userRoleEnum.OWNER) {
    options = options.filter(option => option.value !== reportTypeEnum.TRANSACTION_ERROR);
  }
  if (account === null) {
    options = options.filter(option =>
      option.value === reportTypeEnum.REQUEST_TRANSACTION_REFUND ||
      option.value === reportTypeEnum.TRANSACTION_ERROR);
    isDriver = true;
  }
  switch (account.RoleId) {
    case userRoleEnum.OWNER:
      break;
  }

  const [isTypeRefund, setIsTypeRefund] = useState(false);
  const onChangeReportType = (value) => {
    setIsTypeRefund(value === reportTypeEnum.REQUEST_TRANSACTION_REFUND);
  };

  const reportService = reportServices();
  const onFinish = (value) => {
    value.type = value.type ?? reportTypeEnum.ERROR_DISPLAY;
    reportService.createReport(value);
  };

  return (
    <>
      <div
        className={cx('w-full pl-[20px] pt-[20px] pr-[20px] min-h-[calc(100vh-250px)] mt-[20px] page-account-profile')}>
        <Button className={cx('bg-[#1890FF]')} type='primary' onClick={showModal}>Tạo báo cáo</Button>
      </div>
      <Modal
        confirmLoading={true}
        title='Tạo báo cáo'
        open={isModalOpen}
        okButtonProps={{
          style: {
            backgroundColor: '#1677ff',
            display: 'none',
          },
        }}
        cancelButtonProps={{
          style: {
            display: 'none',
          },
        }}
        onCancel={handleCancel}>
        {/*START form report*/}
        <Form
          labelCol={{ span: 6 }}
          wrapperCol={{ span: 17 }}
          form={form}
          onFinish={onFinish}
        >
          <Form.Item
            label='Loại báo cáo'
            name='type'
          >
            <Select
              className={cx('min-w-[230px]')}
              defaultValue={reportTypeEnum.ERROR_DISPLAY}
              style={{ width: 120 }}
              options={options}
              onChange={onChangeReportType}
            />
          </Form.Item>
          {isDriver === true ? templateDriver : <></>}
          {isTypeRefund === true ? templatePaymentCode : <></>}
          <Form.Item
            label='Nội dung'
            name='content'
            rules={[
              {
                required: true,
                message: 'Không để trống nội dung!',
              },
              {
                max: 1000,
                message: 'Nội dung tối đa 1000 ký tự!',
              },
            ]}
          >
            <TextArea className={'max-h-[97px!important] min-h-[97px!important] w-full'} rows={4}
                      placeholder='Nội dung tối đa 1000 ký tự!' />
          </Form.Item>
          <Form.Item className={('flex justify-center m-0')}>
            <Button className={('bg-[#1890FF] w-[200%]')} type='primary' htmlType={'submit'}>
              Gửi
            </Button>
          </Form.Item>
        </Form>
        {/*END form report*/}
      </Modal>
    </>
  );
}

export default Test;