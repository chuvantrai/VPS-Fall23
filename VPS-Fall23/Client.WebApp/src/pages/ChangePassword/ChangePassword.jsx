import { App, Button, Col, Form, Input, Row } from 'antd';
import { EyeInvisibleOutlined, EyeTwoTone } from '@ant-design/icons';
import { Link, useNavigate } from 'react-router-dom';
import classNames from 'classnames/bind';
import styles from '@/pages/ChangePassword/ChangePassword.module.scss';
import { useAxios } from '@/hooks/index.js';

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

function ChangePassword() {
  const app = App.useApp();
  const navigate = useNavigate();
  const [form] = Form.useForm();
  const axios = useAxios();

  const validateConfirmPassword = (_, value) => {
    if (value && value !== form.getFieldValue('newPassword')) {
      return Promise.reject('Mật khẩu không khớp!');
    }
    return Promise.resolve();
  };

  const validateNewPassword = (_, value) => {
    if (value && value === form.getFieldValue('oldPassword')) {
      return Promise.reject('Mật khẩu mới không được giống mật khẩu cũ!');
    }
    return Promise.resolve();
  };

  const onFinish = (values) => {
    axios.put('/api/Auth/ChangePassword', values)
      .then((res) => {
        if (res.status === 200) {
          app.notification.success({
            message: `Đổi mật khẩu thành công`,
            placement: 'topRight',
          });
          navigate('/login');
        }
      })
  };
  const onClickLogo = () => {
    navigate('/');
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
        <div className={cx('inline-flex flex-col items-center gap-3 mt-[10px] w-full')}>
          <div className={cx('flex justify-center items-center gap-[17.308px] pl-0')}>
            <div className={cx('header-title-logo')}>
              <img src={'../src/assets/logo/logo.png'} alt={'loading...'} onClick={onClickLogo} />
            </div>
          </div>
        </div>
        <div className={cx('flex w-[368px] h-[372px] flex-col items-start gap-[22px] mt-10')}>
          <h5 className={cx('register-form-text  flex flex-col justify-center items-start self-stretch ' +
            'text-[color:var(--character-title-85,rgba(0,0,0,0.85))] text-[16px] not-italic font-medium leading-6')}>
            Đôi mật Khẩu</h5>
          <Form
            className={cx('min-w-[600px]')}
            {...formItemLayout}
            form={form}
            name='changePassword'
            onFinish={onFinish}
            scrollToFirstError
          >
            <Form.Item
              name='oldPassword'
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
                }
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
              name='newPassword'
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
                { validator: validateNewPassword },
              ]}
              hasFeedback
            >
              <Input.Password
                autoComplete='new-password'
                placeholder='Nhập lại mật khẩu'
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
                  Đăng Nhập
                </Link>
              </Col>
            </Row>
          </Form>
        </div>
      </div>
    </div>
  );
}

export default ChangePassword;