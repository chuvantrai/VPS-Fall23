import { App, Button, Col, Divider, Form, Input, Row } from 'antd';
import { EyeInvisibleOutlined, EyeTwoTone } from '@ant-design/icons';
import { Link, useNavigate } from 'react-router-dom';
import classNames from 'classnames/bind';
import styles from '@/pages/ForgotPassword/ForgotPassword.module.scss';
import { useAxios } from '@/hooks/index.js';
import logo from '@/assets/logo/logo.png';
import bgImg from '@/assets/bg.svg';

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

function convertEmailToStars(inputStr) {
  if (inputStr.length >= 5) {
    const stars = Array.from({ length: 5 }, () => '*').join('');
    return inputStr.slice(0, 1) + stars + inputStr.slice(5);
  }
  return inputStr;
}

function ForgotPassword() {
  const app = App.useApp();
  const navigate = useNavigate();
  const [form] = Form.useForm();
  const axios = useAxios();

  const validateConfirmPassword = (_, value) => {
    if (value && value !== form.getFieldValue('password')) {
      return Promise.reject('Mật khẩu không khớp!');
    }
    return Promise.resolve();
  };

  const onFinish = (values) => {
    axios.put('/api/Auth/ForgotPassword', values)
        .then((res) => {
          if (res.status === 200) {
            app.notification.success({
              message: `Đổi mật khẩu thành công`,
              placement: 'topRight',
            });
            navigate(`/login`);
          }
        })
        .catch((error) => {
          console.log(error);
        });
  };
  const onClickLogo = () => {
    navigate('/');
  };

  const SendCodeVerify = () => {
    console.log(123)
    const usernameValue = form.getFieldValue('username');
    if (!usernameValue) {
      app.notification.error({
        message: `Lỗi`,
        description: `Chưa nhập tài khoản/email`,
        placement: 'topRight',
      });
    } else {
      axios.put('/api/Auth/ResendVerificationCode', { userName: usernameValue })
        .then((res) => {
          app.notification.success({
            message: `Kiểm tra email ${convertEmailToStars(res.data)} của bạn để lấy mã xác thực!`,
            placement: 'topRight',
          });
        });
    }

  };

  return (
      <div
          className={cx('bg-[#F0F2F5] w-full min-h-[calc(100vh)] overflow-hidden flex flex-col items-center justify-center')}>
        <div className={cx('bg-img w-full')}>
          <img
              className={cx('w-[100%] relative')}
              src={bgImg}
              alt={'loading...'}
          />
        </div>
        <div className={cx('absolute')}>
          <div className={cx('inline-flex flex-col items-center gap-3 mt-[10px] w-full')}>
            <div className={cx('flex justify-center items-center gap-[17.308px] pl-0')}>
              <div className={cx('header-title-logo')}>
                <img src={logo} alt={'loading...'} onClick={onClickLogo} />
              </div>
            </div>
          </div>
          <div className={cx('flex w-[368px] h-[372px] flex-col items-start gap-[22px] mt-10')}>
            <h5 className={cx('register-form-text  flex flex-col justify-center items-start self-stretch ' +
                'text-[color:var(--character-title-85,rgba(0,0,0,0.85))] text-[16px] not-italic font-medium leading-6')}>
              Quên mật Khẩu</h5>
            <Form
                className={cx('min-w-[600px]')}
                {...formItemLayout}
                form={form}
                name='forgotPassword'
                onFinish={onFinish}
                scrollToFirstError
            >
              <Form.Item
                  name='username'
                  rules={[
                    {
                      required: true,
                      message: 'Hãy nhập tên tài khoản/email của bạn!',
                    },
                  ]}
              >
                <Input placeholder='Tên tài khoản/email' value={''} />
              </Form.Item>
              <div className={cx('flex')}>
                <Form.Item
                    className={cx('w-[52%] verifyCode-formItem')}
                    name='verifyCode'
                    rules={[
                      {
                        required: true,
                        message: 'Hãy code xác nhận!',
                      },
                      {
                        min: 6,
                        message: 'Code xác nhận sai!',
                      },
                      {
                        max: 6,
                        message: 'Code xác nhận sai!',
                      },
                    ]}
                >
                  <Input
                      className={cx('w-[140%]')}
                      placeholder='Code lấy lại mật khẩu'
                  />

                </Form.Item>
                <Button onClick={SendCodeVerify}>Gửi code</Button>
              </div>
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
                    autoComplete='new-password'
                    placeholder='Mật khẩu(6 đến 12 ký tự)'
                    iconRender={(visible) => (visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />)}
                />
              </Form.Item>

              <Form.Item
                  name='confirmPassword'
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
                    { validator: validateConfirmPassword },
                  ]}
                  hasFeedback
              >
                <Input.Password
                    autoComplete='new-password'
                    placeholder='Nhập lại mật khẩu'
                    iconRender={(visible) => (visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />)}
                />
              </Form.Item>
              <Row>
                <Col span={12}>
                  <Form.Item>
                    <Button
                        className={cx('bg-[#1677ff]')}
                        type='primary'
                        htmlType='submit'
                        block
                    >
                      Lưu
                    </Button>
                  </Form.Item>
                </Col>
                <Col span={4} className={'flex justify-center h-[32px] items-center'}>
                  <Link
                      to={'/login'}
                      className={cx('text-[rgb(22,119,255)] inline-flex h-6 justify-center items-center gap-2.5 ' +
                          'shrink-0 rounded-sm text-[\'#1677ff\']')}
                  >
                    Đăng nhập
                  </Link>
                  <Divider orientation='center' type='vertical' />
                  <Link
                      to={'/register'}
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

export default ForgotPassword;