import classNames from "classnames/bind";
import { Button, Form, Input, Row, Col } from 'antd';
import { useLocation, Navigate } from 'react-router-dom';

import styles from './VerifyEmail.module.scss'
import { useAxios } from '@/hooks';
import { useState } from "react";

const cx = classNames.bind(styles)

function VerifyEmail() {
  const [form] = Form.useForm();
  const axios = useAxios()
  const location = useLocation();
  const [verify, setVerify] = useState(false)

  const email = location.state?.email
  const onFinish = (values) => {
    axios.post('api/Auth/VerifyNewAccount', { email, ...values })
      .then(res => {
        if (res.status === 200) {
          setVerify(true)
        }
      })
  };

  const handleResendCode = () => {

  }

  return (
    <div className={cx('wrapper')}>
      <div className={cx('header-title-logo')}>
        <img src="../src/assets/logo/logo.png" />
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
      <div className={cx('verify-form')}>
        <h5 className={cx('verify-form-text')}>Xác thực tài khoản</h5>
        <Form
          form={form}
          name="verify-email"
          onFinish={onFinish}
          scrollToFirstError
        >
          <Form.Item
            name="verifyCode"
            rules={[
              {
                required: true,
                message: 'Hãy nhập Verify Code vào đây!',
              },
              {
                max: 6,
                message: 'Mã code tối đa gồm 6 số'
              },
              {
                pattern: /^[0-9]+$/,
                message: 'Sai định dạng'
              }
            ]}
            style={{
              minWidth: '398px'
            }}
          >
            <Input placeholder="Verify Code" />
          </Form.Item>

          <Row justify="space-between">
            <Col span={11}>
              <Form.Item>
                <Button
                  type="primary"
                  htmlType="submit"
                  block
                  style={{
                    backgroundColor: '#1677ff',
                  }}
                >
                  Xác nhận
                </Button>
              </Form.Item>
            </Col>
            <Col span={11}>
              <Form.Item>
                <Button
                  htmlType="button"
                  block
                  style={{
                    minWidth: '182px'
                  }}
                  onClick={handleResendCode}
                >
                  Gửi lại
                </Button>
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </div>
      {verify === true && <Navigate to="/login" />}
    </div>
  );
}

export default VerifyEmail;