import classNames from 'classnames/bind';
import { Button, Form, Input, Row, Col, notification } from 'antd';
import { EyeInvisibleOutlined, EyeTwoTone } from '@ant-design/icons';
import { Link, Navigate } from 'react-router-dom';

import styles from './Register.module.scss';
import config from '@/config';
import { useAxios } from '@/hooks';
import { useState } from 'react';

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
  const [api, contextHolder] = notification.useNotification();
  const axios = useAxios();
  const [account, setAccount] = useState(null);

  const onFinish = (values) => {
    axios.post('/api/Auth/Register', values)
      .then(res => {
        if (res.status === 200)
          setAccount(values)
      })
      .catch(err => {
        api['error']({
          message: 'Có lỗi xảy ra',
          description: err,
        });
      })
  };

  return (
    <div className={cx('wrapper')}>
      {contextHolder}
      <div className={cx('header')}>
        <div className={cx('header-title')}>
          <div className={cx('header-title-logo')}>
            <img src="../src/assets/logo/logo.png" />
          </div>
        </div>
      </div>
      <div className={cx('bg-img')}>
        <img
          src="../src/assets/bg.svg"
          style={{
            width: '1440px',
            position: 'relative',
          }}
        />
      </div>
      <div className={cx('register-form')}>
        <h5 className={cx('register-form-text')}>Đăng ký</h5>
        <Form
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
            <Input placeholder="Email" />
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
            <Input placeholder="Tên" />
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
            <Input placeholder="Họ" />
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
                message: "Sai định dạng"
              },
            ]}
          >
            <Input placeholder="Số điện thoại" />
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
                  Đăng nhập
                </Link>
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </div>
      {account && <Navigate to="/verifyEmail" replace={true} state={account} />}
    </div>
  );
}

export default Register;
