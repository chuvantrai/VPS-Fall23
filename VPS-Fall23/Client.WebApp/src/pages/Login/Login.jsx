import styles from './Login.module.scss';
import classNames from 'classnames/bind.js';
import { Button, Checkbox, Col, Form, Input, Row } from 'antd';
import { EyeInvisibleOutlined, EyeTwoTone, LockOutlined, UserOutlined } from '@ant-design/icons';
import { Link } from 'react-router-dom';
import config from '@/config/index.js';
import { useAxios } from '@/hooks/index.js';
import Cookies from 'js-cookie'; 
import { convertAccountDataToCode, keyNameCookies } from '@/helpers/index.js';   

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


function Login() {
  Cookies.set(keyNameCookies.ACCESS_TOKEN, '');
  Cookies.set(keyNameCookies.ACCOUNT_DATA, '');
  let rememberPassword = false;
  const [form] = Form.useForm();
  const axios = useAxios();
  const onFinish = (values) => {
    axios.post('/api/Auth/AuthLogin', values)
      .then((res) => {
        Cookies.set(keyNameCookies.ACCESS_TOKEN, res.data.accessToken);
        if (rememberPassword) {
          Cookies.set(keyNameCookies.ACCOUNT_DATA, convertAccountDataToCode(values.username, values.password));
        }
        window.location.href = '/';
      })
      .catch(err => console.log(err));
  };

  const onRememberPassword = (e) => {
    rememberPassword = e.target.checked;
  };

  return (
    <div
      className={cx('bg-[#F0F2F5] w-full min-h-[calc(100vh)] overflow-hidden flex flex-col items-center justify-center')}>
      <div className={cx('bg-img w-full')}>
        <img
          className={cx('w-[100%] relative')}
          src={'../src/assets/bg.svg'}
          alt={'loading...'}
           />
      </div>
      <div className={cx('absolute')}>
        <div className={cx('inline-flex flex-col items-center gap-3 mt-[80px]')}>
          <div className={cx('flex justify-center items-center gap-[17.308px] pl-0')}>
            <div className={cx('header-title-logo')}>
              <img src={'../src/assets/logo/logo.png'} alt={'loading...'} />
            </div>
          </div>
        </div>
        <div className={cx('flex w-[368px] h-[372px] flex-col items-start gap-[22px] mt-10')}>
          <h5 className={cx('register-form-text  flex flex-col justify-center items-start self-stretch ' +
            'text-[color:var(--character-title-85,rgba(0,0,0,0.85))] text-[16px] not-italic font-medium leading-6')}>
            Đăng Nhập</h5>
          <Form
            className={cx('min-w-[600px]')}
            {...formItemLayout}
            form={form}
            name='register'
            onFinish={onFinish}
            scrollToFirstError
          >
            <Form.Item
              name='username'
              rules={[
                {
                  required: true,
                  message: 'Hãy nhập tên tài khoản của bạn!',
                },
              ]}
            >
              <Input placeholder='Tên tài khoản' prefix={<UserOutlined />} />
            </Form.Item>

            <Form.Item
              name='password'
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
                prefix={<LockOutlined />}
                placeholder='Mật khẩu(6 đến 12 ký tự)'
                iconRender={(visible) => (visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />)}
              />
            </Form.Item>
            <Row className={'mb-[24px]'}>
              <Col span={12}>
                <Checkbox onChange={onRememberPassword}>Nhớ tài khoản</Checkbox>
              </Col>
              <Col span={4}>
                <Link
                  to={config.routes.login}
                  className={cx('text-[rgb(22,119,255)] inline-flex h-6 justify-center items-center gap-2.5 ' +
                    'shrink-0 rounded-sm text-[\'#1677ff\']')}
                >
                  Quên mật khẩu
                </Link>
              </Col>
            </Row>
            <Row>
              <Col span={12}>
                <Form.Item>
                  <Button
                    className={cx('bg-[#1677ff]')}
                    type='primary'
                    htmlType='submit'
                    block
                  >
                    Đăng nhập
                  </Button>
                </Form.Item>
              </Col>
              <Col span={4} className={'flex justify-center h-[32px] items-center'}>
                <Link
                  to={config.routes.register}
                  className={cx('text-[rgb(22,119,255)] inline-flex h-6 justify-center items-center gap-2.5 ' +
                    'shrink-0 rounded-sm text-[\'#1677ff\']')}
                >
                  Đăng ký
                </Link>
              </Col>
            </Row>
          </Form>
        </div>
      </div>
    </div>
  );
}

export default Login;
