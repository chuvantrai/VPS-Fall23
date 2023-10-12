/* eslint-disable no-unused-vars */
import classNames from 'classnames/bind';
import { Link, Navigate } from 'react-router-dom';
import { useState } from 'react';
import { Button, Form, Input, Row, Col, DatePicker } from 'antd';
import { EyeInvisibleOutlined, EyeTwoTone } from '@ant-design/icons';

import styles from './Register.module.scss';
import config from '@/config';
import useAuthService from '@/services/authService';

const cx = classNames.bind(styles);
const formItemLayout = {
  labelCol: {
    xs: {
      span: 24,
    },
    sm: {
      span: 8,
    },
  },
  wrapperCol: {
    xs: {
      span: 24,
    },
    sm: {
      span: 16,
    },
  },
};

function Register() {
  const [form] = Form.useForm();
  const authService = useAuthService();
  const [account, setAccount] = useState(null);
  const [dob, setDob] = useState('');

  const onFinish = (values) => {
    values = { ...values, dob };
    authService.register(values, setAccount);
  };

  return (
    <div
      className={cx(
        'bg-[#F0F2F5] w-full min-h-[calc(100vh)] overflow-hidden flex flex-col items-center justify-center',
      )}
    >
      <div className={cx('bg-img w-full')}>
        <img src="../src/assets/bg.svg" className={cx('w-full relative')} />

      </div>
      <div className={cx('absolute')}>
        <div className={cx('inline-flex flex-col items-center gap-3 w-full')}>
          <div className={cx('flex justify-center items-center gap-[17.308px] pl-0')}>
            <div className={cx('header-title-logo')}>
              <img src="../src/assets/logo/logo.png" />
            </div>
          </div>
        </div>
        <div className={cx('flex w-[368px] h-[372px] flex-col items-start gap-[22px] mt-10')}>
          <h5
            className={cx(
              'register-form-text  flex flex-col justify-center items-start self-stretch' +
                'text-[color:var(--character-title-85,rgba(0,0,0,0.85))] text-[16px] not-italic font-medium leading-6',
            )}
          >
            Đăng ký
          </h5>

          <Form
            className={cx('min-w-[600px]')}
            {...formItemLayout}
            form={form}
            name="register"
            onFinish={onFinish}
            style={{
              minWidth: 600,
            }}
            scrollToFirstError
          >
            <Form.Item
              name="email"
              rules={[
                {
                  type: 'email',
                  message: 'Email vừa nhập không hợp lệ!',
                },
                {
                  required: true,
                  message: 'Hãy nhập Email của bạn!',
                },
              ]}
            >
              <Input placeholder="Email" allowClear />
            </Form.Item>

            <Form.Item
              name="password"
              rules={[
                {
                  required: true,
                  message: 'Hãy nhập Mật khẩu của bạn!',
                },
                {
                  min: 6,
                  message: 'Mật khẩu tối thiểu phải có 6 ký tự!',
                },
                {
                  max: 12,
                  message: 'Mật khẩu tối đa là 12 ký tự',
                },
              ]}
            >
              <Input.Password
                placeholder="Mật khẩu(6 đến 12 ký tự)"
                iconRender={(visible) => (visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />)}
                allowClear
              />
            </Form.Item>

            <Form.Item
              name="firstName"
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
              <Input placeholder="Tên" allowClear />
            </Form.Item>

            <Form.Item
              name="lastName"
              rules={[
                {
                  required: true,
                  message: 'Hãy điền họ của bạn!',
                },
                {
                  max: 20,
                  message: 'Họ tối đa chỉ được 20 ký tự!',
                },
              ]}
            >
              <Input placeholder="Họ" allowClear />
            </Form.Item>

            <Form.Item name="dob">
              <DatePicker
                onChange={(_, dateString) => setDob(dateString)}
                style={{
                  minWidth: '400px',
                }}
                placeholder="Ngày sinh"
              />
            </Form.Item>

            <Form.Item
              name="phoneNumber"
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
              <Input placeholder="Số điện thoại" allowClear />
            </Form.Item>

            <Row>
              <Col span={12}>
                <Form.Item>
                  <Button
                    type="primary"
                    htmlType="submit"
                    block
                    style={{
                      backgroundColor: '#1677ff',
                    }}
                  >
                    Đăng ký
                  </Button>
                </Form.Item>
              </Col>
              <Col span={12}>
                <Form.Item>
                  <Link
                    to={config.routes.login}
                    className={cx('login-link')}
                    style={{
                      color: '#1677ff',
                    }}
                  >
                    Đã có tài khoản? Đăng nhập
                  </Link>
                </Form.Item>
              </Col>
            </Row>
          </Form>
        </div>

        {account && <Navigate to={config.routes.verifyEmail} replace={true} state={account} />}
      </div>
    </div>
  );
}

export default Register;
