require File.dirname(__FILE__) + '/../test_helper'

class ContainersControllerTest < ActionController::TestCase
  def test_should_get_index
    get :index
    assert_response :success
    assert_not_nil assigns(:containers)
  end

  def test_should_get_new
    get :new
    assert_response :success
  end

  def test_should_create_container
    assert_difference('Container.count') do
      post :create, :container => { }
    end

    assert_redirected_to container_path(assigns(:container))
  end

  def test_should_show_container
    get :show, :id => 1
    assert_response :success
  end

  def test_should_get_edit
    get :edit, :id => 1
    assert_response :success
  end

  def test_should_update_container
    put :update, :id => 1, :container => { }
    assert_redirected_to container_path(assigns(:container))
  end

  def test_should_destroy_container
    assert_difference('Container.count', -1) do
      delete :destroy, :id => 1
    end

    assert_redirected_to containers_path
  end
end
