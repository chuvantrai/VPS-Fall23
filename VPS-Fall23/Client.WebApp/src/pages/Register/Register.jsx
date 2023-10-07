import classNames from 'classnames/bind';
import { Button, Form, Input, Row, Col } from 'antd';
import { EyeInvisibleOutlined, EyeTwoTone } from '@ant-design/icons';
import { Link } from 'react-router-dom';
import 'react-toastify/dist/ReactToastify.css';

import styles from './Register.module.scss';
import config from '@/config';
import { useAxios } from '@/hooks';

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
  const axios = useAxios();
  const onFinish = (values) => {
    axios
      .post('/api/Auth/Register', values)
      .then((res) => console.log(res))
      .catch((err) => console.log(err));
  };

  return (
    <div className={cx('wrapper')}>
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
            hasFeedback
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
    </div>
  );
}

export default Register;
